﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DataIngestion.DB.Interfaces;
using DataIngestion.DB.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            try
            {
                if (artist == null) return false;
                await using var entities = new DataIngestionContext();
                if (entities.Artist.Any(o => o.ArtistId == artist.ArtistId)) return false;

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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException sqlex)
            {
                _logger.LogError(sqlex.Message, sqlex.Message, artist);
                return false;
            }
        }

        public async Task<bool> InsertArtistCollection(ArtistCollection artistCollection)
        {
            try
            {
                if (artistCollection == null) return false;
                await using var entities = new DataIngestionContext();
                if (entities.ArtistCollection.Any(o => o.CollectionId == artistCollection.CollectionId)) return false;

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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException sqlex)
            {
                _logger.LogError(sqlex.Message, sqlex.Message, artistCollection);
                return false;
            }
        }

        public async Task<bool> InsertCollection(Collection collection)
        {
            try
            {
                if (collection == null) return false;
                await using var entities = new DataIngestionContext();
                if (await entities.Collection.AnyAsync(o => o.CollectionId == collection.CollectionId).ConfigureAwait(true)) return false;

                await entities.Collection.AddAsync(collection).ConfigureAwait(true);
                var saveChangesAsync = await entities.SaveChangesAsync().ConfigureAwait(true);

                if (saveChangesAsync > 0)
                {
                    _logger.LogInformation("Saved 1 collection.");

                    return true;
                }

                _logger.LogInformation("Saved 0 collection.");
                return false;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException sqlex)
            {
                _logger.LogError(sqlex.Message, sqlex.Message, collection);
                return false;
            }
        }

        public async Task<bool> InsertCollectionMatch(CollectionMatch collectionMatch)
        {
            try
            {
                if (collectionMatch == null) return false;
                await using var entities = new DataIngestionContext();
                if (entities.CollectionMatch.Any(o => o.CollectionId == collectionMatch.CollectionId)) return false;

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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException sqlex)
            {
                _logger.LogError(sqlex.Message, sqlex.Message, collectionMatch);
                return false;
            }
        }

        #endregion
    }
}