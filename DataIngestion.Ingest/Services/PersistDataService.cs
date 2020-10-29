using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Models;
using DataIngestion.Ingest.Interfaces;
using DataIngestion.Ingest.Models;
using Microsoft.Extensions.Configuration;

namespace DataIngestion.Ingest.Services
{
	public class PersistDataService : IPersistDataService
	{
		#region Fields

		private readonly IConfigurationRoot _configurationRoot;
		private readonly IDataIngestionRepository _dataIngestionRepository;

		#endregion

		#region Constructor

		public PersistDataService(IDataIngestionRepository dataIngestionRepository,
			IConfigurationRoot configurationRoot)
		{
			_dataIngestionRepository = dataIngestionRepository ??
			                           throw new ArgumentNullException(nameof(dataIngestionRepository));
			_configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
		}

		#endregion

		#region Public Methods

		public bool PersistData()
		{
			try
			{
				var googleDriveFiles = _configurationRoot.GetSection("GoogleDriveFiles").GetChildren();
				var destPath = _configurationRoot.GetValue<string>("UnZip:ExtractToPathPath");

				var taskArray = new Task[googleDriveFiles.Count()];
				var taskNumber = 0;

				var watch = new Stopwatch();
				watch.Start();

				foreach (var configurationSection in googleDriveFiles)
				{
					taskArray[taskNumber] = Task.Factory.StartNew(async obj =>
					{
						if (!(obj is CustomData data))
							return;

						var sourceFileName = $"{destPath}\\{configurationSection.Key}\\tar";
						var row = 0;

						// Open the file to read from.
						using (var sr = File.OpenText(sourceFileName))
						{
							string line;
							while ((line = await sr.ReadLineAsync().ConfigureAwait(true)) != null)
							{
								row++;
								if (row <= 3)
									continue;

								//do minimal amount of work here 
								if (configurationSection.Key.Equals("Artist"))
								{
									await _dataIngestionRepository.InsertArtist(GetArtistRow(line))
										.ConfigureAwait(true);
									continue;
								}

								if (configurationSection.Key.Equals("ArtistCollection"))
								{
									await _dataIngestionRepository.InsertArtistCollection(GetArtistCollectionRow(line))
										.ConfigureAwait(true);
									continue;
								}

								if (configurationSection.Key.Equals("Collection"))
								{
									await _dataIngestionRepository.InsertCollection(GetCollectionRow(line))
										.ConfigureAwait(true);
									continue;
								}

								if (!configurationSection.Key.Equals("CollectionMatch")) continue;

								await _dataIngestionRepository.InsertCollectionMatch(GetCollectionMatchRow(line))
									.ConfigureAwait(true);
							}
						}

						data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
						data.Name = configurationSection.Key;
						data.TotalRows = row;
					}, new CustomData {CreationTime = DateTime.Now.Ticks});

					taskNumber++;
				}

				Console.WriteLine("Persisting date in Database please wait.");

				Task.WaitAll(taskArray);

				watch.Stop();

				var timeSpan = watch.Elapsed;

				foreach (var task in taskArray)
					if (task.AsyncState is CustomData data)
						Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
							data.Name, data.CreationTime, data.ThreadNum);

				Console.WriteLine("Write data to DB toke: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes,
					timeSpan.Seconds, timeSpan.Milliseconds);

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

		private Artist GetArtistRow(string row)
		{
			var replace = row.Replace("\u0002", "");
			var strings = replace.Split('\x01');

			var artist = new Artist
			{
				ExportDate = strings[0],
				ArtistId = double.Parse(!string.IsNullOrEmpty(strings[1]) ? strings[1] : "0"),
				Name = strings[2],
				IsActualArtist = strings[3],
				ViewUrl = strings[4],
				ArtistTypeId = strings[5]
			};

			return artist;
		}

		private ArtistCollection GetArtistCollectionRow(string row)
		{
			var replace = row.Replace("\u0002", "");
			var strings = replace.Split('\x01');

			var artistCollection = new ArtistCollection
			{
				ExportDate = strings[0],
				ArtistId = double.Parse(!string.IsNullOrEmpty(strings[1]) ? strings[1] : "0"),
				CollectionId = int.Parse(!string.IsNullOrEmpty(strings[2]) ? strings[2] : "0"),
				IsPrimaryArtist = strings[3],
				RoleId = strings[4]
			};

			return artistCollection;
		}

		private Collection GetCollectionRow(string row)
		{
			var replace = row.Replace("\u0002", "");
			var strings = replace.Split('\x01');

			var collection = new Collection
			{
				ExportDate = strings[0],
				CollectionId = long.Parse(!string.IsNullOrEmpty(strings[1]) ? strings[1] : "0"),
				Name = strings[2],
				TitleVersion = strings[3],
				SearchTerms = strings[4],
				ParentalAdvisoryId = strings[5],
				ArtistDisplayName = strings[6],
				ViewUrl = strings[7],
				ArtworkUrl = strings[8],
				OriginalReleaseDate = strings[9],
				ItunesReleaseDate = strings[10],
				LabelStudio = strings[11],
				ContentProviderName = strings[12],
				Copyright = strings[13],
				PLine = strings[14],
				MediaTypeId = strings[15],
				IsCompilation = strings[16],
				CollectionTypeId = strings[17],
			};

			return collection;
		}

		private CollectionMatch GetCollectionMatchRow(string row)
		{
			var replace = row.Replace("\u0002", "");
			var strings = replace.Split('\x01');

			var collectionMatch = new CollectionMatch
			{
				ExportDate = strings[0],
				CollectionId = int.Parse(!string.IsNullOrEmpty(strings[1]) ? strings[1] : "0"),
				Upc = double.Parse(!string.IsNullOrEmpty(strings[2]) ? strings[2] : "0"),
				Grid = strings[3],
				AmgAlbumId = strings[4]
			};

			return collectionMatch;
		}

		#endregion
	}
}