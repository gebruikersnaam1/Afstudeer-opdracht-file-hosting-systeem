using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProofOfConceptServer.entities;

namespace ProofOfConceptServer.Models
{
    public partial class woefiedatabaseContext : DbContext
    {
        public woefiedatabaseContext()
        {
        }

        public woefiedatabaseContext(DbContextOptions<woefiedatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BlobItem> BlobItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=woefieserver.database.windows.net;Initial Catalog=woefiedatabase;Persist Security Info=True;User ID=woefiebeheerder;Password=LN7T7sGkhBjYz4eF");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlobItem>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__blobItem__C2C6FFFC19A5FE79");

                entity.ToTable("blobItem");

                entity.Property(e => e.FileId)
                    .HasColumnName("fileID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("fileName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileSize)
                    .IsRequired()
                    .HasColumnName("fileSize")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PathFile)
                    .IsRequired()
                    .HasColumnName("pathFile")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
