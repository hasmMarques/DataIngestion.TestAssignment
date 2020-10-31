namespace DataIngestion.DB.Models
{
	public partial class CollectionMatch
	{
		#region Properties

		public string ExportDate { get; set; }
		public long CollectionId { get; set; }
		public string Upc { get; set; }
		public string Grid { get; set; }
		public string AmgAlbumId { get; set; }

		#endregion
	}
}