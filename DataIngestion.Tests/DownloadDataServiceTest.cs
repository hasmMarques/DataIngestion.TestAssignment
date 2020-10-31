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
	public class DownloadDataServiceTest
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
			var extractDataService = new DownloadDataService(configuration);

			//Assert
			extractDataService.DownloadZipFile().Should().BeFalse("the url are not valid");
		}

		#endregion
	}
}