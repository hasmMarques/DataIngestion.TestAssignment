using System;
using System.IO;
using System.Threading.Tasks;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Repository;
using DataIngestion.Ingest.Interfaces;
using DataIngestion.Ingest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataIngestion.TestAssignment
{
	class Program
	{
		#region Fields

		private static ServiceProvider serviceProvider;
		private static ILogger<Program> logger;

		#endregion

		#region Private Methods

		static async Task Main(string[] args)
		{
			RegisterServices(); //Register the needed services

			//Downloads the file from google drive
			DownloadZipFiles();

			//Extracts the files 
			ExtractaData();

			//reads and saves the data into the database
			PersistData();

			//Reads from database and injects the data into Elasticsearch
			await SendToElasticsearch().ConfigureAwait(true);

			logger.LogDebug("All done!");

			Console.ReadKey();
		}

		private static async Task SendToElasticsearch()
		{
			var elasticSearch = serviceProvider.GetService<IElasticSearchService>();
			var result = await elasticSearch.Import().ConfigureAwait(true);
			if (!result)
			{
				Console.WriteLine("Persisting data from DB into Elasticsearch did not complete successfully");
				Console.ReadKey();
			}
		}

		private static void PersistData()
		{
			var persistDataService = serviceProvider.GetService<IPersistDataService>();
			var result = persistDataService.PersistData1();
			if (result) return;
			Console.WriteLine("Persisting data from files into DB did not complete successfully");
			Console.ReadKey();
		}

		private static void ExtractaData()
		{
			var extractDataService = serviceProvider.GetService<IExtractDataService>();
			var result = extractDataService.Extract();
			if (result) return;
			Console.WriteLine("Extracting files did not complete successfully");
			Console.ReadKey();
		}

		private static void DownloadZipFiles()
		{
			var downloadZipFile = serviceProvider.GetService<IDownloadDataService>();
			var result = downloadZipFile.DownloadZipFile();
			if (result) return;
			Console.WriteLine("Download files did not complete successfully");
			Console.ReadKey();
		}

		private static void RegisterServices()
		{
			//setup our DI
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging()
				.AddSingleton<IDataIngestionRepository, DataIngestionRepository>();

			// Build configuration
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
				.AddJsonFile("appsettings.json", false)
				.Build();

			// Add generic IConfiguration
			serviceCollection.AddSingleton<IConfiguration>(configuration);
			serviceCollection.AddSingleton<IDownloadDataService, DownloadDataService>();
			serviceCollection.AddSingleton<IExtractDataService, ExtractDataService>();
			serviceCollection.AddSingleton<IPersistDataService, PersistDataService>();
			serviceCollection.AddSingleton<IElasticSearchService, ElasticSearchService>();

			serviceProvider = serviceCollection.BuildServiceProvider();

			serviceProvider
				.GetService<ILoggerFactory>();

			logger = serviceProvider.GetService<ILoggerFactory>()
				.CreateLogger<Program>();

			logger.LogDebug("Starting application");
		}

		#endregion
	}
}