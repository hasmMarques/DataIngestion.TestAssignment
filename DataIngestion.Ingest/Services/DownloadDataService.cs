using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using DataIngestion.Ingest.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
	public class DownloadDataService : IDownloadDataService
	{
		#region Fields

		private readonly IConfigurationRoot _configurationRoot;

		private bool finished;
		private FileDownloader fileDownloader;

		#endregion

		#region Constructor

		public DownloadDataService(IConfigurationRoot configurationRoot)
		{
			_configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
		}

		#endregion

		#region Public Methods

		public bool DownloadZipFile()
		{
            try
            {
                InitiateFileDownloader();

                Download();

                return true;
            }
            catch (HttpRequestException ex)
            {
	            Console.WriteLine(ex.Message);
				return false;
            }
		}

		private void Download()
		{
			var googleDriveFiles = _configurationRoot.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = _configurationRoot.GetValue<string>("DestinationPath");
			
			Console.WriteLine("Downloading files");
			foreach (var configurationSection in googleDriveFiles)
			{
				finished = false;
				fileDownloader.DownloadFileAsync(configurationSection.Value,
					$"{destPath}{configurationSection.Key}.tbz");

				while (!finished)
				{
				}
			}
		}

		private void InitiateFileDownloader()
		{
			// NOTE: FileDownloader is IDisposable!
			fileDownloader = new FileDownloader();

			// This callback is triggered for DownloadFileAsync only
			fileDownloader.DownloadProgressChanged += FileDownloaderOnDownloadProgressChanged;

			// This callback is triggered for both DownloadFile and DownloadFileAsync
			fileDownloader.DownloadFileCompleted += FileDownloaderOnDownloadFileCompleted;
		}

		#endregion

		#region Private Methods

		private void FileDownloaderOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			finished = true;
			Console.WriteLine(" - Download completed");
		}

		private void FileDownloaderOnDownloadProgressChanged(object sender, FileDownloader.DownloadProgress progress)
		{
			Console.Write(
				$"\r{progress.FileName} {progress.ProgressPercentage}% {FormatBytes(progress.TotalBytesToReceive)}");
		}

		private string FormatBytes(long bytes)
		{
			string[] Suffix = {"B", "KB", "MB", "GB", "TB"};
			int i;
			double dblSByte = bytes;
			for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024) dblSByte = bytes / 1024.0;

			return $"{dblSByte:0.##} {Suffix[i]}   ";
		}

		#endregion
	}
}