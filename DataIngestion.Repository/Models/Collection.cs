using System;
using System.Collections.Generic;

namespace DataIngestion.DB.Models
{
    public partial class Collection
    {
        public string ExportDate { get; set; }
        public string CollectionId { get; set; }
        public string Name { get; set; }
        public string TitleVersion { get; set; }
        public string SearchTerms { get; set; }
        public string ParentalAdvisoryId { get; set; }
        public string ArtistDisplayName { get; set; }
        public string ViewUrl { get; set; }
        public string ArtworkUrl { get; set; }
        public string OriginalReleaseDate { get; set; }
        public string ItunesReleaseDate { get; set; }
        public string LabelStudio { get; set; }
        public string ContentProviderName { get; set; }
        public string Copyright { get; set; }
        public string PLine { get; set; }
        public string MediaTypeId { get; set; }
        public string IsCompilation { get; set; }
        public string CollectionTypeId { get; set; }
    }
}
