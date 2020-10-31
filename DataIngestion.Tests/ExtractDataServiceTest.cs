using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataIngestion.Ingest.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataIngestion.Tests
{
	[TestClass]
	public class ExtractDataServiceTest
	{
		#region Public Methods

		[TestMethod]
		public void TestConfiguration0()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};
			//Act
			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			//Assert
			configuration.GetSection("Download").GetChildren().Count().Should().Be(1);
			configuration.GetSection("UnZip").GetChildren().Count().Should().Be(1);
			configuration.GetSection("GoogleDriveFiles").GetChildren().Count().Should().Be(4);
		}

		[TestMethod]
		public void TestExtractPositive()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.Extract().Should().BeTrue("the path contain all the necessary files");
		}

		[TestMethod]
		public void TestExtractFalse()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.Extract().Should().BeFalse("the path doesn't contain all the necessary files");
		}
		
		[TestMethod]
		public void TestTBZFilesExistPositive2()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var startPath = configuration.GetValue<string>("Download:DestinationPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfCompressedTBZFilesExist(googleDriveFiles, startPath).Should()
				.Be(true, "the path contains all the necessary files");
		}

		[TestMethod]
		public void TestTBZFilesExistNegative3()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var startPath = configuration.GetValue<string>("Download:DestinationPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfCompressedTBZFilesExist(googleDriveFiles, startPath).Should()
				.Be(false, "the path doesn't contain all the necessary files");
		}

		[TestMethod]
		public void TestCompressedTARFilesExistPositive4()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = configuration.GetValue<string>("UnZip:ExtractToPathPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfCompressedTARFilesExist(googleDriveFiles, destPath).Should()
				.Be(true, "the path contains all the necessary files");
		}

		[TestMethod]
		public void TestCompressedTARFilesExistNegative5()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = configuration.GetValue<string>("UnZip:ExtractToPathPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfCompressedTARFilesExist(googleDriveFiles, destPath).Should()
				.Be(true, "the path doesn't contain all the needed files");
		}

		[TestMethod]
		public void TestTARFilesExistPositive6()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = configuration.GetValue<string>("UnZip:ExtractToPathPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfTARFilesExist(googleDriveFiles, destPath).Should()
				.Be(true, "the path contains all the necessary files");
		}

		[TestMethod]
		public void TestTARFilesExistNegative7()
		{
			//Arrange
			var inMemorySettings =
				new Dictionary<string, string>
				{
					{"Download:DestinationPath", Path.Combine(Directory.GetCurrentDirectory(), @"")},
					{
						"UnZip:ExtractToPathPath",
						Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Data\FilesToDigest")
					},
					{"GoogleDriveFiles:Collection", "Collection"},
					{"GoogleDriveFiles:ArtistCollection", "ArtistCollection"},
					{"GoogleDriveFiles:CollectionMatch", "CollectionMatch"},
					{"GoogleDriveFiles:Artist", "Artist"},
				};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

			var googleDriveFiles = configuration.GetSection("GoogleDriveFiles").GetChildren();
			var destPath = configuration.GetValue<string>("UnZip:ExtractToPathPath");
			
			//Act
			var extractDataService = new ExtractDataService(configuration);

			//Assert
			extractDataService.CheckIfTARFilesExist(googleDriveFiles, destPath).Should()
				.Be(true, "the path doesn't contain all the needed files");
		}

		#endregion
	}
}