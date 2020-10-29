using System;
using System.Threading.Tasks;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Models;
using Microsoft.Extensions.Logging;

//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=DataIngestion;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

namespace DataIngestion.DB.Repository
{
    public class DataIngestionRepository : IDataIngestionRepository
    {
        #region Fields

        private readonly ILogger<DataIngestionRepository> _logger;

        #endregion

        #region Constructor

        public DataIngestionRepository(ILoggerFactory loggerFactory)
        {
            var factory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = factory.CreateLogger<DataIngestionRepository>();
        }

        #endregion

        #region Public Methods

        public async Task<bool> InsertArtist(Artist artist)
        {
            if (artist == null) throw new ArgumentNullException(nameof(artist));
            await using var entities = new DataIngestionContext();

            await entities.Artist.AddAsync(artist).ConfigureAwait(true);
            var saveChanges = await entities.SaveChangesAsync().ConfigureAwait(true);

            if (saveChanges > 0)
            {
                _logger.LogInformation($"Saved {artist.Name} artistCollection.");

                return true;
            }

            _logger.LogInformation("Saved 0 artist.");
            return false;
        }

        public async Task<bool> InsertArtistCollection(ArtistCollection artistCollection)
        {
            if (artistCollection == null) throw new ArgumentNullException(nameof(artistCollection));
            await using var entities = new DataIngestionContext();

            await entities.ArtistCollection.AddAsync(artistCollection);
            var saveChangesAsync = await entities.SaveChangesAsync().ConfigureAwait(true);

            if (saveChangesAsync > 0)
            {
                _logger.LogInformation("Saved 1 artistCollection.");

                return true;
            }

            _logger.LogInformation("Saved 0 artistCollection.");
            return false;
        }

        public async Task<bool> InsertCollection(Collection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            await using var entities = new DataIngestionContext();

            await entities.Collection.AddAsync(collection);
            var saveChangesAsync = await entities.SaveChangesAsync().ConfigureAwait(true);

            if (saveChangesAsync > 0)
            {
                _logger.LogInformation("Saved 1 collection.");

                return true;
            }

            _logger.LogInformation("Saved 0 collection.");
            return false;
        }

        public async Task<bool> InsertCollectionMatch(CollectionMatch collectionMatch)
        {
            if (collectionMatch == null) throw new ArgumentNullException(nameof(collectionMatch));
            await using var entities = new DataIngestionContext();

            await entities.CollectionMatch.AddAsync(collectionMatch);
            var saveChangesAsync = await entities.SaveChangesAsync().ConfigureAwait(true);

            if (saveChangesAsync > 0)
            {
                _logger.LogInformation("Saved 1 collectionMatch.");

                return true;
            }

            _logger.LogInformation("Saved 0 collectionMatch.");
            return false;
        }

        #endregion
    }
}