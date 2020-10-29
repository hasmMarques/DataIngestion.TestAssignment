using System;

namespace DataIngestion.DB.Dtos
{
	public class Album
	{
		#region Properties

		public string Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Upc { get; set; }
		public string ReleaseDate { get; set; }
		public string IsCompilation { get; set; }
		public string Label { get; set; }
		public string ImageUrl { get; set; }
		public Artist[] Artists { get; set; }

		#endregion
	}
}