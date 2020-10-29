using System;
using System.Collections.Generic;

namespace DataIngestion.DB.Models
{
    public partial class CollectionMatch
    {
        public string ExportDate { get; set; }
        public int CollectionId { get; set; }
        public double Upc { get; set; }
        public string Grid { get; set; }
        public string AmgAlbumId { get; set; }
    }
}
