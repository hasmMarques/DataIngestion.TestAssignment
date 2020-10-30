using System;
using System.IO;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Repository;
using DataIngestion.Ingest.Interfaces;
using DataIngestion.Ingest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using LogLevel = Nest.LogLevel;

namespace DataIngestion.TestAssignment
{
	class Program
	{
		#region Private Methods

		static void Main(string[] args)
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

			// Add generic IConfigurationRoot
			serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
			serviceCollection.AddSingleton<IDownloadDataService, DownloadDataService>();
			serviceCollection.AddSingleton<IExtractDataService, ExtractDataService>();
			serviceCollection.AddSingleton<IPersistDataService, PersistDataService>();
			
			IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
			//configure console logging
			serviceProvider
				.GetService<ILoggerFactory>();

			var logger = serviceProvider.GetService<ILoggerFactory>()
				.CreateLogger<Program>();
			logger.LogDebug("Starting application");

            //do the actual work here
            //var downloadZipFile = serviceProvider.GetService<IDownloadDataService>();
            //downloadZipFile.DownloadZipFile();
            //var extractDataService = serviceProvider.GetService<IExtractDataService>();
            //extractDataService.Extract();
            var persistDataService = serviceProvider.GetService<IPersistDataService>();
            persistDataService.PersistData1();

            logger.LogDebug("All done!");

			Console.ReadKey();
		}

		private static void InsertIntoElastic()
		{
			var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
				.DefaultIndex("people");

			var client = new ElasticClient(settings);

			var person = new Person
			{
				Id = 1,
				FirstName = "Martijn",
				LastName = "Laarman"
			};

			var indexResponse = client.IndexDocument(person);


			var searchResponse = client.Search<Person>(s => s
				.From(0)
				.Size(10)
				.Query(q => q
					.Match(m => m
						.Field(f => f.FirstName)
						.Query("Martijn")
					)
				)
			);

			var people = searchResponse.Documents;
		}

		#endregion
	}

	public class Person
	{
		#region Properties

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		#endregion
	}
}