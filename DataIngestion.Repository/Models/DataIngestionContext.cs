using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataIngestion.DB.Models
{
    public partial class DataIngestionContext : DbContext
    {
        public DataIngestionContext()
        {
        }

        public DataIngestionContext(DbContextOptions<DataIngestionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<ArtistCollection> ArtistCollection { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<CollectionMatch> CollectionMatch { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO:move to vault or to appsettings.json
                //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DataIngestion;Integrated Security=True");
                optionsBuilder.UseSqlServer("Server=DESKTOP-RAISED2\\SQLEXPRESS;Database=DataIngestion;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.Property(e => e.ArtistId)
                    .HasColumnName("artistId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ArtistTypeId)
                    .HasColumnName("artistTypeId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExportDate)
                    .HasColumnName("exportDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActualArtist)
                    .HasColumnName("isActualArtist")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsUnicode(false);

                entity.Property(e => e.ViewUrl)
                    .HasColumnName("viewUrl")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArtistCollection>(entity =>
            {
                entity.HasKey(e => new { e.ArtistId, e.CollectionId, e.RoleId })
                    .HasName("PK__ArtistCo__B132EB54FE1BA026");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("artistId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CollectionId)
                    .HasColumnName("collectionId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasColumnName("roleId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExportDate)
                    .HasColumnName("exportDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsPrimaryArtist)
                    .HasColumnName("isPrimaryArtist")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.CollectionId)
                    .HasColumnName("collectionId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ArtistDisplayName)
                    .HasColumnName("artistDisplayName")
                    .IsUnicode(false);

                entity.Property(e => e.ArtworkUrl)
                    .HasColumnName("artworkUrl")
                    .IsUnicode(false);

                entity.Property(e => e.CollectionTypeId)
                    .HasColumnName("collectionTypeId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContentProviderName)
                    .HasColumnName("contentProviderName")
                    .IsUnicode(false);

                entity.Property(e => e.Copyright)
                    .HasColumnName("copyright")
                    .IsUnicode(false);

                entity.Property(e => e.ExportDate)
                    .HasColumnName("exportDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsCompilation)
                    .HasColumnName("isCompilation")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ItunesReleaseDate)
                    .HasColumnName("itunesReleaseDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LabelStudio)
                    .HasColumnName("labelStudio")
                    .IsUnicode(false);

                entity.Property(e => e.MediaTypeId)
                    .HasColumnName("mediaTypeId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsUnicode(false);

                entity.Property(e => e.OriginalReleaseDate)
                    .HasColumnName("originalReleaseDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PLine)
                    .HasColumnName("pLine")
                    .IsUnicode(false);

                entity.Property(e => e.ParentalAdvisoryId)
                    .HasColumnName("parentalAdvisoryId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SearchTerms)
                    .HasColumnName("searchTerms")
                    .IsUnicode(false);

                entity.Property(e => e.TitleVersion)
                    .HasColumnName("titleVersion")
                    .IsUnicode(false);

                entity.Property(e => e.ViewUrl)
                    .HasColumnName("viewUrl")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CollectionMatch>(entity =>
            {
                entity.HasKey(e => e.CollectionId)
                    .HasName("PK__Collecti__5BCE195CA72FD56C");

                entity.Property(e => e.CollectionId)
                    .HasColumnName("collectionId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AmgAlbumId)
                    .HasColumnName("amgAlbumId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExportDate)
                    .HasColumnName("exportDate")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Grid)
                    .HasColumnName("grid")
                    .IsUnicode(false);

                entity.Property(e => e.Upc)
                    .HasColumnName("upc")
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
