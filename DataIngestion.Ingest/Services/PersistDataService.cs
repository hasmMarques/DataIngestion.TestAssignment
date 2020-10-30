using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Models;
using DataIngestion.Ingest.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Task = System.Threading.Tasks.Task;

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

		public bool PersistData2()
		{
			try
			{
				var watch = new Stopwatch();
				watch.Start();

				Console.WriteLine("Persisting data in Database please wait.");

				var eventListener = new MyEventListener();

                var pkgLocation = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ImportToSql\\Package.dtsx");
                var app = new Application();
				var dtsPackage100 = app.LoadPackage(pkgLocation, true, eventListener);
				var pkgResults = dtsPackage100.Execute(null, null, eventListener, null, null);  
  
				Console.WriteLine(pkgResults.ToString());  
				Console.ReadKey();  


				watch.Stop();

				var timeSpan = watch.Elapsed;

				Console.WriteLine("Writing data to DB toke: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes,
					timeSpan.Seconds, timeSpan.Milliseconds);

				return true;
			}
			catch (IOException ioex)
			{
				Console.WriteLine(ioex.Message);
				return false;
			}
		}

		public bool PersistData1()
		{
			try
			{
				var googleDriveFiles = _configurationRoot.GetSection("GoogleDriveFiles").GetChildren();
				var destPath = _configurationRoot.GetValue<string>("UnZip:ExtractToPathPath");

				var taskArray = new Task[googleDriveFiles.Count()];
				var taskNumber = 0;

				var watch = new Stopwatch();
				watch.Start();

				Console.WriteLine("Persisting data in Database please wait.");
				foreach (var configurationSection in googleDriveFiles)
				{
					taskArray[taskNumber] = Task.Run(async () =>
						await DoIt(destPath, configurationSection).ConfigureAwait(true));
					taskNumber++;
				}

				Task.WaitAll(taskArray);

				watch.Stop();

				var timeSpan = watch.Elapsed;

				Console.WriteLine("Writing data to DB toke: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes,
					timeSpan.Seconds, timeSpan.Milliseconds);

				return true;
			}
			catch (IOException ioex)
			{
				Console.WriteLine(ioex.Message);
				return false;
			}
		}

		public async Task DoIt(string destPath, IConfigurationSection configurationSection)
		{
			var sourceFileName = Directory.GetFiles($"{destPath}\\{configurationSection.Key}").FirstOrDefault();
			var row = 0;

			// Open the file to read from.
			using var sr = File.OpenText(sourceFileName);
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
					await _dataIngestionRepository.InsertCollection(GetCollectionRow(line)).ConfigureAwait(true);
					continue;
				}

				if (!configurationSection.Key.Equals("CollectionMatch")) continue;

				await _dataIngestionRepository.InsertCollectionMatch(GetCollectionMatchRow(line))
					.ConfigureAwait(true);
			}
		}

		#endregion

		#region Private Methods

		private Artist GetArtistRow(string row)
		{
			try
			{
				var replace = row.Replace("\u0002", "");
				var strings = replace.Split('\x01');

				var artist = new Artist
				{
					ExportDate = strings.ElementAtOrDefault(0) != null ? strings[0] : string.Empty,
					ArtistId = strings.ElementAtOrDefault(1) != null ? strings[1] : string.Empty,
					Name = strings.ElementAtOrDefault(2) != null ? strings[2] : string.Empty,
					IsActualArtist = strings.ElementAtOrDefault(3) != null ? strings[3] : string.Empty,
					ViewUrl = strings.ElementAtOrDefault(4) != null ? strings[4] : string.Empty,
					ArtistTypeId = strings.ElementAtOrDefault(5) != null ? strings[5] : string.Empty,
				};

				return artist;
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}

		private ArtistCollection GetArtistCollectionRow(string row)
		{
			try
			{
				var replace = row.Replace("\u0002", "");
				var strings = replace.Split('\x01');

				var artistCollection = new ArtistCollection
				{
					ExportDate = strings.ElementAtOrDefault(0) != null ? strings[0] : string.Empty,
					ArtistId = strings.ElementAtOrDefault(1) != null ? strings[1] : string.Empty,
					CollectionId = strings.ElementAtOrDefault(2) != null ? strings[2] : string.Empty,
					IsPrimaryArtist = strings.ElementAtOrDefault(3) != null ? strings[3] : string.Empty,
					RoleId = strings.ElementAtOrDefault(4) != null ? strings[4] : string.Empty
				};

				return artistCollection;
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}

		private Collection GetCollectionRow(string row)
		{
			try
			{
				var replace = row.Replace("\u0002", "");
				var strings = replace.Split('\x01');

				var collection = new Collection
				{
					ExportDate = strings.ElementAtOrDefault(0) != null ? strings[0] : string.Empty,
					CollectionId = strings.ElementAtOrDefault(1) != null ? strings[1] : string.Empty,
					Name = strings.ElementAtOrDefault(2) != null ? strings[2] : string.Empty,
					TitleVersion = strings.ElementAtOrDefault(3) != null ? strings[3] : string.Empty,
					SearchTerms = strings.ElementAtOrDefault(4) != null ? strings[4] : string.Empty,
					ParentalAdvisoryId = strings.ElementAtOrDefault(5) != null ? strings[5] : string.Empty,
					ArtistDisplayName = strings.ElementAtOrDefault(6) != null ? strings[6] : string.Empty,
					ViewUrl = strings.ElementAtOrDefault(7) != null ? strings[7] : string.Empty,
					ArtworkUrl = strings.ElementAtOrDefault(8) != null ? strings[8] : string.Empty,
					OriginalReleaseDate = strings.ElementAtOrDefault(9) != null ? strings[9] : string.Empty,
					ItunesReleaseDate = strings.ElementAtOrDefault(10) != null ? strings[10] : string.Empty,
					LabelStudio = strings.ElementAtOrDefault(11) != null ? strings[11] : string.Empty,
					ContentProviderName = strings.ElementAtOrDefault(12) != null ? strings[12] : string.Empty,
					Copyright = strings.ElementAtOrDefault(13) != null ? strings[13] : string.Empty,
					PLine = strings.ElementAtOrDefault(14) != null ? strings[14] : string.Empty,
					MediaTypeId = strings.ElementAtOrDefault(15) != null ? strings[15] : string.Empty,
					IsCompilation = strings.ElementAtOrDefault(16) != null ? strings[16] : string.Empty,
					CollectionTypeId = strings.ElementAtOrDefault(17) != null ? strings[17] : string.Empty,
				};

				return collection;
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}

		private CollectionMatch GetCollectionMatchRow(string row)
		{
			try
			{
				var replace = row.Replace("\u0002", "");
				var strings = replace.Split('\x01');

				var collectionMatch = new CollectionMatch
				{
					ExportDate = strings.ElementAtOrDefault(0) != null? strings[0]:string.Empty,
					CollectionId = strings.ElementAtOrDefault(1) != null ? strings[1] : string.Empty,
					Upc = strings.ElementAtOrDefault(2) != null ? strings[2] : string.Empty,
					Grid = strings.ElementAtOrDefault(3) != null ? strings[3] : string.Empty,
					AmgAlbumId = strings.ElementAtOrDefault(4) != null ? strings[4] : string.Empty
				};

				return collectionMatch;
			}
			catch (IndexOutOfRangeException)
			{
				return null;
			}
		}

		#endregion
	}

	public class MyEventListener : IDTSEvents100  
	{
		public void OnPreExecute(IDTSExecutable100 pExec, ref bool pbFireAgain)
		{
			
		}

		public void OnPostExecute(IDTSExecutable100 pExec, ref bool pbFireAgain)
		{
			
		}

		public void OnPreValidate(IDTSExecutable100 pExec, ref bool pbFireAgain)
		{
			
		}

		public void OnPostValidate(IDTSExecutable100 pExec, ref bool pbFireAgain)
		{
			
		}

		public void OnWarning(IDTSRuntimeObject100 pSource, int WarningCode, string SubComponent, string Description, string HelpFile,
			int HelpContext, string IDOfInterfaceWithError)
		{
			
		}

		public void OnInformation(IDTSRuntimeObject100 pSource, int InformationCode, string SubComponent, string Description,
			string HelpFile, int HelpContext, string IDOfInterfaceWithError, ref bool pbFireAgain)
		{
			
		}

		public void OnError(IDTSRuntimeObject100 pSource, int ErrorCode, string SubComponent, string Description, string HelpFile,
			int HelpContext, string IDOfInterfaceWithError, out bool pbCancel)
		{
			pbCancel = false;
			// Add application-specific diagnostics here.  
			Console.WriteLine("Error in {0}/{1} : {2}", pSource, SubComponent, Description);
			var res = pbCancel;
		}

		public void OnTaskFailed(IDTSTaskHost100 pTaskHost)
		{
			
		}

		public void OnProgress(IDTSTaskHost100 pTaskHost, string ProgressDescription, int PercentComplete, int ProgressCountLow,
			int ProgressCountHigh, string SubComponent, ref bool pbFireAgain)
		{
			
		}

		public void OnQueryCancel(out bool pbCancel)
		{
			pbCancel = false;
			var res = pbCancel;
		}

		public void OnBreakpointHit(IDTSBreakpointSite100 pBreakpointSite, IDTSBreakpointTarget100 pBreakpointTarget)
		{
			
		}

		public void OnExecutionStatusChanged(IDTSExecutable100 pExec, DTSExecStatus newStatus, ref bool pbFireAgain)
		{
			
		}

		public void OnVariableValueChanged(IDTSContainer100 pContainer, IDTSVariable100 pVariable, ref bool pbFireAgain)
		{
			
		}

		public void OnCustomEvent(IDTSTaskHost100 pTaskHost, string EventName, string EventText, ref object[] ppsaArguments,
			string SubComponent, ref bool pbFireAgain)
		{
			
		}
	}  
}