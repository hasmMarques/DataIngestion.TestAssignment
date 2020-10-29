using System;
using System.Collections.Generic;
using System.IO;
using DataIngestion.Ingest.Interfaces;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
	public class ExtractDataService : IExtractDataService
	{
		#region Fields

		private readonly IConfigurationRoot _configurationRoot;

		#endregion

		#region Constructor

		public ExtractDataService(IConfigurationRoot configurationRoot)
		{
			_configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
		}

		#endregion

		#region Public Methods

		public bool Extract()
		{
			try
			{
				var googleDriveFiles = _configurationRoot.GetSection("GoogleDriveFiles").GetChildren();
				var startPath = _configurationRoot.GetValue<string>("DestinationPath");
				var destPath = _configurationRoot.GetValue<string>("UnZip:ExtractToPathPath");

				CreateDirIfMissing(destPath);

				ExtractTBZ(googleDriveFiles, startPath, destPath);

				ExtractTar(googleDriveFiles, destPath);

				return true;
			}
			catch (IOException ioex)
			{
				Console.WriteLine(ioex.Message);
				return false;
			}
		}

		#endregion

		#region Private Methods

		private void ExtractTar(IEnumerable<IConfigurationSection> googleDriveFiles, string destPath)
		{
			foreach (var configurationSection in googleDriveFiles)
			{
				var sourceFileName = $"{destPath}\\{configurationSection.Key}.tar";
				var destFileName = $"{destPath}\\{configurationSection.Key}";
				ExtractTar(sourceFileName, destFileName);

				Console.WriteLine($"\rDecompressing {sourceFileName} - Done");
			}
		}

		private void ExtractTBZ(IEnumerable<IConfigurationSection> googleDriveFiles, string startPath, string destPath)
		{
			foreach (var configurationSection in googleDriveFiles)
			{
				var sourceFileName = $"{startPath}\\{configurationSection.Key}.tbz";
				var destFileName = $"{destPath}\\{configurationSection.Key}.tar";
				ExtractTGZ(sourceFileName, destFileName);

				Console.Write($"\rDecompressing {sourceFileName} - Done");
			}
		}

		private void ExtractTar(string tarFileName, string destFolder)
		{
			Console.WriteLine(
				$"Decompressing {tarFileName}");
			Stream inStream = File.OpenRead(tarFileName);

			var tarArchive = TarArchive.CreateInputTarArchive(inStream);
			tarArchive.ExtractContents(destFolder);
			tarArchive.Close();

			inStream.Close();
		}

		private void ExtractTGZ(string gzArchiveName, string destFolder)
		{
			var zipFileName = new FileInfo(gzArchiveName);
			using var fileToDecompressAsStream = zipFileName.OpenRead();
			var decompressedFileName = destFolder;
			using var decompressedStream = File.Create(decompressedFileName);
			try
			{
				BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
				Console.WriteLine(
					$"Decompressing {gzArchiveName}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void CreateDirIfMissing(string path)
		{
			try
			{
				if (!Directory.Exists(path))
				{
					// Try to create the directory.
					var di = Directory.CreateDirectory(path);
				}
			}
			catch (IOException ioex)
			{
				Console.WriteLine(ioex.Message);
			}
		}

		#endregion
	}
}