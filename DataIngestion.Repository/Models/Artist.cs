namespace DataIngestion.DB.Models
{
	public class Artist
	{
		#region Properties

		public string ExportDate { get; set; }
		public long ArtistId { get; set; }
		public string Name { get; set; }
		public string IsActualArtist { get; set; }
		public string ViewUrl { get; set; }
		public string ArtistTypeId { get; set; }

		#endregion
	}
}