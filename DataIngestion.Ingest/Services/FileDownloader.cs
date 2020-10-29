using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

/* EXAMPLE USAGE
	FileDownloader fileDownloader = new FileDownloader();
	// This callback is triggered for DownloadFileAsync only
	fileDownloader.DownloadProgressChanged += ( sender, e ) => Console.WriteLine( "Progress changed " + e.BytesReceived + " " + e.TotalBytesToReceive );
	// This callback is triggered for both DownloadFile and DownloadFileAsync
	fileDownloader.DownloadFileCompleted += ( sender, e ) => Console.WriteLine( "Download completed" );
	fileDownloader.DownloadFileAsync( "https://INSERT_DOWNLOAD_LINK_HERE", @"C:\downloadedFile.txt" );
*/
namespace DataIngestion.Ingest.Services
{
	public class FileDownloader : IDisposable
	{
		#region Fields

		public delegate void DownloadProgressChangedEventHandler(object sender, DownloadProgress progress);

		private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";
		private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";

		// In the worst case, it is necessary to send 3 download requests to the Drive address
		//   1. an NID cookie is returned instead of a download_warning cookie
		//   2. download_warning cookie returned
		//   3. the actual file is downloaded
		private const int GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT = 3;
		private readonly DownloadProgress downloadProgress;

		private readonly CookieAwareWebClient webClient;

		private bool asyncDownload;

		private Uri downloadAddress;

		private bool downloadingDriveFile;
		private string downloadPath;
		private int driveDownloadAttempt;
		private object userToken;

		#endregion

		#region Constructor

		public FileDownloader()
		{
			webClient = new CookieAwareWebClient();
			webClient.DownloadProgressChanged += DownloadProgressChangedCallback;
			webClient.DownloadFileCompleted += DownloadFileCompletedCallback;

			downloadProgress = new DownloadProgress();
		}

		#endregion

		#region Events

		public event DownloadProgressChangedEventHandler DownloadProgressChanged;
		public event AsyncCompletedEventHandler DownloadFileCompleted;

		#endregion

		#region Public Methods

		public void DownloadFile(string address, string fileName)
		{
			DownloadFile(address, fileName, false, null);
		}

		public void DownloadFileAsync(string address, string fileName, object userToken = null)
		{
			DownloadFile(address, fileName, true, userToken);
		}

		public void Dispose()
		{
			webClient.Dispose();
		}

		#endregion

		#region Private Methods

		private void DownloadFile(string address, string fileName, bool asyncDownload, object userToken)
		{
			downloadingDriveFile = address.StartsWith(GOOGLE_DRIVE_DOMAIN) || address.StartsWith(GOOGLE_DRIVE_DOMAIN2);
			if (downloadingDriveFile)
			{
				address = GetGoogleDriveDownloadAddress(address);
				driveDownloadAttempt = 1;

				webClient.ContentRangeTarget = downloadProgress;
			}
			else
			{
				webClient.ContentRangeTarget = null;
			}

			downloadAddress = new Uri(address);
			downloadPath = fileName;

			downloadProgress.TotalBytesToReceive = -1L;
			downloadProgress.UserState = userToken;

			this.asyncDownload = asyncDownload;
			this.userToken = userToken;

			DownloadFileInternal();
		}

		private void DownloadFileInternal()
		{
			if (!asyncDownload)
			{
				webClient.DownloadFile(downloadAddress, downloadPath);

				// This callback isn't triggered for synchronous downloads, manually trigger it
				DownloadFileCompletedCallback(webClient, new AsyncCompletedEventArgs(null, false, null));
			}
			else if (userToken == null)
			{
				webClient.DownloadFileAsync(downloadAddress, downloadPath);
			}
			else
			{
				webClient.DownloadFileAsync(downloadAddress, downloadPath, userToken);
			}
		}

		private void DownloadProgressChangedCallback(object sender, DownloadProgressChangedEventArgs e)
		{
			if (DownloadProgressChanged != null)
			{
				downloadProgress.BytesReceived = e.BytesReceived;
				if (e.TotalBytesToReceive > 0L)
					downloadProgress.TotalBytesToReceive = e.TotalBytesToReceive;

				downloadProgress.FileName = downloadPath.Split("\\")[1];

				DownloadProgressChanged(this, downloadProgress);
			}
		}

		private void DownloadFileCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			if (!downloadingDriveFile)
			{
				if (DownloadFileCompleted != null)
					DownloadFileCompleted(this, e);
			}
			else
			{
				if (driveDownloadAttempt < GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT && !ProcessDriveDownload())
				{
					// Try downloading the Drive file again
					driveDownloadAttempt++;
					DownloadFileInternal();
				}
				else if (DownloadFileCompleted != null)
				{
					DownloadFileCompleted(this, e);
				}
			}
		}

		// Downloading large files from Google Drive prompts a warning screen and requires manual confirmation
		// Consider that case and try to confirm the download automatically if warning prompt occurs
		// Returns true, if no more download requests are necessary
		private bool ProcessDriveDownload()
		{
			var downloadedFile = new FileInfo(downloadPath);
			if (downloadedFile == null)
				return true;

			// Confirmation page is around 50KB, shouldn't be larger than 60KB
			if (downloadedFile.Length > 60000L)
				return true;

			// Downloaded file might be the confirmation page, check it
			string content;
			using (var reader = downloadedFile.OpenText())
			{
				// Confirmation page starts with <!DOCTYPE html>, which can be preceeded by a newline
				var header = new char[20];
				var readCount = reader.ReadBlock(header, 0, 20);
				if (readCount < 20 || !(new string(header).Contains("<!DOCTYPE html>")))
					return true;

				content = reader.ReadToEnd();
			}

			var linkIndex = content.LastIndexOf("href=\"/uc?");
			if (linkIndex < 0)
				return true;

			linkIndex += 6;
			var linkEnd = content.IndexOf('"', linkIndex);
			if (linkEnd < 0)
				return true;

			downloadAddress = new Uri("https://drive.google.com" +
			                          content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&"));
			return false;
		}

		// Handles the following formats (links can be preceeded by https://):
		// - drive.google.com/open?id=FILEID
		// - drive.google.com/file/d/FILEID/view?usp=sharing
		// - drive.google.com/uc?id=FILEID&export=download
		private string GetGoogleDriveDownloadAddress(string address)
		{
			var index = address.IndexOf("id=");
			int closingIndex;
			if (index > 0)
			{
				index += 3;
				closingIndex = address.IndexOf('&', index);
				if (closingIndex < 0)
					closingIndex = address.Length;
			}
			else
			{
				index = address.IndexOf("file/d/");
				if (index < 0) // address is not in any of the supported forms
					return string.Empty;

				index += 7;

				closingIndex = address.IndexOf('/', index);
				if (closingIndex < 0)
				{
					closingIndex = address.IndexOf('?', index);
					if (closingIndex < 0)
						closingIndex = address.Length;
				}
			}

			return string.Concat("https://drive.google.com/uc?id=", address.Substring(index, closingIndex - index),
				"&export=download");
		}

		#endregion

		// Custom download progress reporting (needed for Google Drive)
		public class DownloadProgress
		{
			#region Fields

			public long BytesReceived, TotalBytesToReceive;
			public object UserState;
			public string FileName ;
			#endregion

			#region Properties

			public int ProgressPercentage
			{
				get
				{
					if (TotalBytesToReceive > 0L)
						return (int) (((double) BytesReceived / TotalBytesToReceive) * 100);

					return 0;
				}
			}

			#endregion
		}

		// Web client that preserves cookies (needed for Google Drive)
		private class CookieAwareWebClient : WebClient
		{
			#region Fields

			private readonly CookieContainer cookies = new CookieContainer();
			public DownloadProgress ContentRangeTarget;

			#endregion

			#region Protected Methods

			protected override WebRequest GetWebRequest(Uri address)
			{
				var request = base.GetWebRequest(address);
				if (request is HttpWebRequest)
				{
					var cookie = cookies[address];
					if (cookie != null)
						((HttpWebRequest) request).Headers.Set("cookie", cookie);

					if (ContentRangeTarget != null)
						((HttpWebRequest) request).AddRange(0);
				}

				return request;
			}

			protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
			{
				return ProcessResponse(base.GetWebResponse(request, result));
			}

			protected override WebResponse GetWebResponse(WebRequest request)
			{
				return ProcessResponse(base.GetWebResponse(request));
			}

			#endregion

			#region Private Methods

			private WebResponse ProcessResponse(WebResponse response)
			{
				var cookies = response.Headers.GetValues("Set-Cookie");
				if (cookies != null && cookies.Length > 0)
				{
					var length = 0;
					for (var i = 0; i < cookies.Length; i++)
						length += cookies[i].Length;

					var cookie = new StringBuilder(length);
					for (var i = 0; i < cookies.Length; i++)
						cookie.Append(cookies[i]);

					this.cookies[response.ResponseUri] = cookie.ToString();
				}

				if (ContentRangeTarget != null)
				{
					var rangeLengthHeader = response.Headers.GetValues("Content-Range");
					if (rangeLengthHeader != null && rangeLengthHeader.Length > 0)
					{
						var splitIndex = rangeLengthHeader[0].LastIndexOf('/');
						if (splitIndex >= 0 && splitIndex < rangeLengthHeader[0].Length - 1)
						{
							long length;
							if (long.TryParse(rangeLengthHeader[0].Substring(splitIndex + 1), out length))
								ContentRangeTarget.TotalBytesToReceive = length;
						}
					}
				}

				return response;
			}

			#endregion

			private class CookieContainer
			{
				#region Fields

				private readonly Dictionary<string, string> cookies = new Dictionary<string, string>();

				#endregion

				#region Properties

				public string this[Uri address]
				{
					get
					{
						string cookie;
						if (cookies.TryGetValue(address.Host, out cookie))
							return cookie;

						return null;
					}
					set { cookies[address.Host] = value; }
				}

				#endregion
			}
		}
	}
}