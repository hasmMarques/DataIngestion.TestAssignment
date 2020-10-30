using System;
using System.Collections.Generic;

namespace DataIngestion.DB.Models
{
    public partial class CollectionMatch
    {
        public string ExportDate { get; set; }
        public string CollectionId { get; set; }
        public string Upc { get; set; }
        public string Grid { get; set; }
        public string AmgAlbumId { get; set; }
    }
}
