using System;
using System.Collections.Generic;

namespace DataIngestion.DB.Models
{
    public partial class Artist
    {
        public string ExportDate { get; set; }
        public double ArtistId { get; set; }
        public string Name { get; set; }
        public string IsActualArtist { get; set; }
        public string ViewUrl { get; set; }
        public string ArtistTypeId { get; set; }
    }
}
