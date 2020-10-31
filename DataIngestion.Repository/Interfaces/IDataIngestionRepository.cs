using System.Collections.Generic;
using System.Threading.Tasks;
using DataIngestion.DB.Dtos;
using DataIngestion.DB.Models;

namespace DataIngestion.DB.Interfaces
{
	public interface IDataIngestionRepository
	{
		#region Public Methods

		Task<List<Album>> GetAlbums(int skip, int take);
		Task<List<AlbumArtist>> GetArtists(long collectionId);
		Task<int> GetTotalRowsOfAlbums();
		Task<bool> InsertArtist(Artist artist);
		Task<bool> InsertArtistCollection(ArtistCollection artistCollection);
		Task<bool> InsertCollection(Collection collection);
		Task<bool> InsertCollectionMatch(CollectionMatch collectionMatch);

		#endregion
	}
}