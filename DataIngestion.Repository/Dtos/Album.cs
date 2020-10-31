using System.Collections.Generic;

namespace DataIngestion.DB.Dtos
{
	public partial class Album
	{
		#region Properties

		public long AlbumId { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Upc { get; set; }
		public string ReleaseDate { get; set; }
		public string IsCompilation { get; set; }
		public string Label { get; set; }
		public string ImageUrl { get; set; }
		public long IdAlbumArtist { get; set; }
		public List<AlbumArtist> Artist { get; set; }

		#endregion
	}
}