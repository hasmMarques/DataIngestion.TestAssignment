using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataIngestion.Ingest.Interfaces;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
	public class ExtractDataService : IExtractDataService
	{
		#region Fields

		private readonly IConfiguration _configurationRoot;

		#endregion

		#region Constructor

		public ExtractDataService(IConfiguration configuration)
		{
			_configurationRoot = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#endregion

		#region Public Methods

		public bool Extract()
		{
			try
			{
				Console.WriteLine("Decompressing files");
				var googleDriveFiles = _configurationRoot.GetSection("GoogleDriveFiles").GetChildren();
				var startPath = _configurationRoot.GetValue<string>("Download:DestinationPath");
				var destPath = _configurationRoot.GetValue<string>("UnZip:ExtractToPathPath");


				if (!CheckIfCompressedTBZFilesExist(googleDriveFiles, startPath))
				{
					Console.WriteLine("Error: One or more compressed *.TBZ files are missing.");
					return false;
				}

				CreateDirIfMissing(destPath);

				ExtractTBZ(googleDriveFiles, startPath, destPath);

				if (!CheckIfCompressedTARFilesExist(googleDriveFiles, destPath))
				{
					Console.WriteLine("Error: One or more compressed *.TAR files are missing.");
					return false;
				}

				ExtractTar(googleDriveFiles, destPath);

				return true;
			}
			catch (IOException ioex)
			{
				Console.WriteLine(ioex.Message);
				return false;
			}
		}

		public bool CheckIfCompressedTBZFilesExist(IEnumerable<IConfigurationSection> googleDriveFiles,
			string startPath)
		{
			return googleDriveFiles.Select(configurationSection => $"{startPath}\\{configurationSection.Key}.tbz")
				.All(File.Exists);
		}

		public bool CheckIfCompressedTARFilesExist(IEnumerable<IConfigurationSection> googleDriveFiles, string destPath)
		{
			return googleDriveFiles.Select(configurationSection => $"{destPath}\\{configurationSection.Key}.tar")
				.All(File.Exists);
		}

		public bool CheckIfTARFilesExist(IEnumerable<IConfigurationSection> googleDriveFiles, string destPath)
		{
			return googleDriveFiles.Select(configurationSection =>
				$"{destPath}\\{configurationSection.Key}\\{configurationSection.Key}\\tar").All(File.Exists);
		}

		public void ExtractTar(IEnumerable<IConfigurationSection> googleDriveFiles, string destPath)
		{
			foreach (var configurationSection in googleDriveFiles)
			{
				var sourceFileName = $"{destPath}\\{configurationSection.Key}.tar";
				var destFileName = $"{destPath}\\{configurationSection.Key}";
				ExtractTar(sourceFileName, destFileName);

				Console.WriteLine($"\rDecompressing {sourceFileName} - Done");
			}
		}

		public void ExtractTBZ(IEnumerable<IConfigurationSection> googleDriveFiles, string startPath, string destPath)
		{
			foreach (var configurationSection in googleDriveFiles)
			{
				var sourceFileName = $"{startPath}\\{configurationSection.Key}.tbz";
				var destFileName = $"{destPath}\\{configurationSection.Key}.tar";
				ExtractTGZ(sourceFileName, destFileName);

				Console.Write($"\rDecompressing {sourceFileName} - Done {Environment.NewLine}");
			}
		}

		#endregion

		#region Private Methods

		private void ExtractTar(string tarFileName, string destFolder)
		{
			try
			{
				Console.Write(
					$"\rDecompressing {tarFileName}");
				Stream inStream = File.OpenRead(tarFileName);

				var tarArchive = TarArchive.CreateInputTarArchive(inStream);
				tarArchive.ExtractContents(destFolder);
				tarArchive.Close();

				inStream.Close();
			}
			catch (IOException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void ExtractTGZ(string gzArchiveName, string destFolder)
		{
			try
			{
				var zipFileName = new FileInfo(gzArchiveName);
				using var fileToDecompressAsStream = zipFileName.OpenRead();
				var decompressedFileName = destFolder;
				using var decompressedStream = File.Create(decompressedFileName);

				Console.Write(
					$"\rDecompressing {gzArchiveName}");
				BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
			}
			catch (IOException ex)
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