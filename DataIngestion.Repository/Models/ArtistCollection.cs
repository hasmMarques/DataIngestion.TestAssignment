namespace DataIngestion.DB.Models
{
	public partial class ArtistCollection
	{
		#region Properties

		public string ExportDate { get; set; }
		public long ArtistId { get; set; }
		public long CollectionId { get; set; }
		public string IsPrimaryArtist { get; set; }
		public string RoleId { get; set; }

		#endregion
	}
}