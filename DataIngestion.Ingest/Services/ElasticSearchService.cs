using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataIngestion.DB.Dtos;
using DataIngestion.DB.Interfaces;
using DataIngestion.Ingest.Interfaces;
using Microsoft.Extensions.Configuration;
using Nest;

namespace DataIngestion.Ingest.Services
{
	public class ElasticSearchService : IElasticSearchService
	{
		#region Fields

		private readonly IConfiguration _configuration;
		private readonly IDataIngestionRepository _dataIngestionRepository;

		#endregion

		#region Constructor

		public ElasticSearchService(IDataIngestionRepository dataIngestionRepository,
			IConfiguration configuration)
		{
			_dataIngestionRepository = dataIngestionRepository ??
			                           throw new ArgumentNullException(nameof(dataIngestionRepository));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#endregion

		#region Public Methods

		public async Task<bool> Import()
		{
			var totalRowsOfAlbums = await _dataIngestionRepository.GetTotalRowsOfAlbums().ConfigureAwait(true);
			var totalCalls = totalRowsOfAlbums > 100 ? 50 : 1;
			var totalPerCall = totalRowsOfAlbums / totalCalls;
			var skipRows = 0;

			var watch = new Stopwatch();
			watch.Start();

			Console.WriteLine("Persisting data in ElasticSearch");

			for (var i = 0; i < totalCalls; i++)
			{
				await InsertoIntoElastic(await GetAlbums(skipRows, totalPerCall).ConfigureAwait(true))
					.ConfigureAwait(true);
				skipRows += totalPerCall;
			}

			Console.WriteLine("Persisting data in ElasticSearch DONE");

			return true;
		}

		#endregion

		#region Private Methods

		private async Task<List<Album>> GetAlbums(int skipRows, int totalPerCall)
		{
			var albums = await _dataIngestionRepository.GetAlbums(skipRows, totalPerCall).ConfigureAwait(true);

			var tasks = albums.Select(async album =>
			{
				var response = album.Artist =
					new List<AlbumArtist>(await _dataIngestionRepository.GetArtists(album.IdAlbumArtist)
						.ConfigureAwait(true));
			});
			await Task.WhenAll(tasks);

			return albums;
		}

		private async Task<bool> InsertoIntoElastic(List<Album> albums)
		{
			//TODO:move uri to vault or to appsettings.json
			var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
				.DefaultIndex("album");

			var client = new ElasticClient(settings);

			var bag = new ConcurrentBag<object>();
			var tasks = albums.Select(async item =>
			{
				var response = await client.IndexDocumentAsync(item);
				bag.Add(response);
			});
			await Task.WhenAll(tasks);
			var count = bag.Count;

			var searchResponse = client.Search<Album>(s => s
				.From(0)
				.Size(10)
				.Query(q => q
					.Match(m => m
						.Field(f => f.AlbumId)
						.Query("880642640")
					)
				)
			);

			return true;
		}

		#endregion
	}
}