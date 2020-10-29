using System.Collections.Generic;
using System.Threading.Tasks;
using DataIngestion.DB.Models;

namespace DataIngestion.DB.Interfaces
{
    public interface IDataIngestionRepository
    {
        Task<bool> InsertArtist(Artist artist);
        Task<bool> InsertArtistCollection(ArtistCollection artistCollection);
        Task<bool> InsertCollection(Collection collection);
        Task<bool> InsertCollectionMatch(CollectionMatch collectionMatch);
    }
}