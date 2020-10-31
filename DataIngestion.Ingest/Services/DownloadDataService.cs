using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using DataIngestion.Ingest.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
	public class DownloadDataService : IDownloadDataService
	{
		#region Fields

		private readonly IConfiguration _configuration;
		private FileDownloader fileDownloader;

		private bool finished;

		#endregion

		#region Constructor

		public DownloadDataService(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#endregion

		#region Public Methods

		public bool DownloadZipFile()
		{
			try
			{
				InitiateFileDownloader();

				return Download();
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		#endregion

		#region Private Methods

		private bool Download()
		{
			var googleDriveFiles = _configuration.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = _configuration.GetValue<string>("Download:DestinationPath");

			if (!ValidUrls(googleDriveFiles))
			{
				Console.WriteLine("One or more url are invalid");
				return false;
			}

			Console.WriteLine("Downloading files");
			foreach (var configurationSection in googleDriveFiles)
			{
				finished = false;
				fileDownloader.DownloadFileAsync(configurationSection.Value,
					$"{destPath}{configurationSection.Key}.tbz");

				while (!finished) { }
			}

			return true;
		}

		private bool ValidUrls(IEnumerable<IConfigurationSection> googleDriveFiles)
		{
            foreach (var configurationSection in googleDriveFiles)
            {
	            if (!Uri.IsWellFormedUriString(configurationSection.Value, UriKind.Absolute))
		            return false;

	            if (!Uri.TryCreate(configurationSection.Value, UriKind.Absolute, out var uriResult)
	                && (uriResult?.Scheme == Uri.UriSchemeHttp || uriResult?.Scheme == Uri.UriSchemeHttps))
		            return false;
            }

            return true;
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