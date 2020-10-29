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
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DataIngestion;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.Property(e => e.ArtistId).HasColumnName("artist_id");

                entity.Property(e => e.ArtistTypeId)
                    .IsRequired()
                    .HasColumnName("artist_type_id")
                    .HasMaxLength(50);

                entity.Property(e => e.ExportDate)
                    .IsRequired()
                    .HasColumnName("export_date")
                    .HasMaxLength(50);

                entity.Property(e => e.IsActualArtist)
                    .IsRequired()
                    .HasColumnName("is_actual_artist")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.ViewUrl)
                    .IsRequired()
                    .HasColumnName("view_url")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ArtistCollection>(entity =>
            {
                entity.HasKey(e => new { e.ArtistId, e.CollectionId, e.RoleId })
                    .HasName("PK__ArtistCo__149B73385A960388");

                entity.Property(e => e.ArtistId).HasColumnName("artist_id");

                entity.Property(e => e.CollectionId).HasColumnName("collection_id");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasMaxLength(50);

                entity.Property(e => e.ExportDate)
                    .IsRequired()
                    .HasColumnName("export_date")
                    .HasMaxLength(50);

                entity.Property(e => e.IsPrimaryArtist)
                    .IsRequired()
                    .HasColumnName("is_primary_artist")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.CollectionId)
                    .HasColumnName("collection_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ArtistDisplayName)
                    .HasColumnName("artist_display_name")
                    .HasMaxLength(500);

                entity.Property(e => e.ArtworkUrl)
                    .HasColumnName("artwork_url")
                    .HasMaxLength(500);

                entity.Property(e => e.CollectionTypeId)
                    .HasColumnName("collection_type_id")
                    .HasMaxLength(50);

                entity.Property(e => e.ContentProviderName)
                    .HasColumnName("content_provider_name")
                    .HasMaxLength(500);

                entity.Property(e => e.Copyright)
                    .HasColumnName("copyright")
                    .HasMaxLength(500);

                entity.Property(e => e.ExportDate)
                    .HasColumnName("export_date")
                    .HasMaxLength(50);

                entity.Property(e => e.IsCompilation)
                    .HasColumnName("is_compilation")
                    .HasMaxLength(50);

                entity.Property(e => e.ItunesReleaseDate)
                    .HasColumnName("itunes_release_date")
                    .HasMaxLength(50);

                entity.Property(e => e.LabelStudio)
                    .HasColumnName("label_studio")
                    .HasMaxLength(500);

                entity.Property(e => e.MediaTypeId)
                    .HasColumnName("media_type_id")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.OriginalReleaseDate)
                    .HasColumnName("original_release_date")
                    .HasMaxLength(50);

                entity.Property(e => e.PLine)
                    .HasColumnName("p_line")
                    .HasMaxLength(500);

                entity.Property(e => e.ParentalAdvisoryId)
                    .HasColumnName("parental_advisory_id")
                    .HasMaxLength(50);

                entity.Property(e => e.SearchTerms)
                    .HasColumnName("search_terms")
                    .HasMaxLength(500);

                entity.Property(e => e.TitleVersion)
                    .HasColumnName("title_version")
                    .HasMaxLength(500);

                entity.Property(e => e.ViewUrl)
                    .HasColumnName("view_url")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<CollectionMatch>(entity =>
            {
                entity.HasKey(e => e.CollectionId)
                    .HasName("PK__Collecti__53D3A5CA5DB30531");

                entity.Property(e => e.CollectionId)
                    .HasColumnName("collection_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AmgAlbumId)
                    .IsRequired()
                    .HasColumnName("amg_album_id")
                    .HasMaxLength(50);

                entity.Property(e => e.ExportDate)
                    .IsRequired()
                    .HasColumnName("export_date")
                    .HasMaxLength(50);

                entity.Property(e => e.Grid)
                    .IsRequired()
                    .HasColumnName("grid")
                    .HasMaxLength(500);

                entity.Property(e => e.Upc).HasColumnName("upc");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
