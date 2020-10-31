using System;
using System.IO;
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

		static void Main(string[] args)
		{
			RegisterServices();

			//var downloadZipFile = serviceProvider.GetService<IDownloadDataService>();
			//downloadZipFile.DownloadZipFile();

			var extractDataService = serviceProvider.GetService<IExtractDataService>();
			extractDataService.Extract();

			//var persistDataService = serviceProvider.GetService<IPersistDataService>();
			//persistDataService.PersistData1();

			//var elasticSearch = serviceProvider.GetService<IElasticSearchService>();
			//elasticSearch.Import();

			logger.LogDebug("All done!");

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