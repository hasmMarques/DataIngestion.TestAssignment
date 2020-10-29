using System;
using System.Collections.Generic;

namespace DataIngestion.DB.Models
{
    public partial class ArtistCollection
    {
        public string ExportDate { get; set; }
        public double ArtistId { get; set; }
        public int CollectionId { get; set; }
        public string IsPrimaryArtist { get; set; }
        public string RoleId { get; set; }
    }
}
