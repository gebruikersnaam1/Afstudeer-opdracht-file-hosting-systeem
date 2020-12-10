using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProofOfConceptServer.Repositories.entities;

namespace ProofOfConceptServer.Repositories.models
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
        public virtual DbSet<FolderItem> FolderItems { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<ShareItem> ShareItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=woefieserver.database.windows.net;Initial Catalog=woefiedatabase;Persist Security Info=True;User ID=woefiebeheerder;Password=LN7T7sGkhBjYz4eF");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlobItem>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK__tmp_ms_x__C2C6FFFC1150BD38");

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

                entity.Property(e => e.FileSize).HasColumnName("fileSize");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FolderItem>(entity =>
            {
                entity.HasKey(e => e.BlobId)
                    .HasName("PK__FolderIt__F4CC75F8B556F2E3");

                entity.Property(e => e.BlobId)
                    .HasColumnName("blobID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FolderId).HasColumnName("folderID");

                entity.HasOne(d => d.Blob)
                    .WithOne(p => p.FolderItems)
                    .HasForeignKey<FolderItem>(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FolderItems_T");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.FolderItems)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_ToTable");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(e => e.FolderId)
                    .HasName("PK__folders__C2FABF93CA580360");

                entity.ToTable("folders");

                entity.Property(e => e.FolderId)
                    .HasColumnName("folderId")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.DateChanged)
                    .HasColumnName("dateChanged")
                    .HasColumnType("date");

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasColumnName("folderName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ParentFolder).HasColumnName("parentFolder");
            });

            modelBuilder.Entity<ShareItem>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActiveUntil)
                    .HasColumnName("activeUntil")
                    .HasColumnType("date");

                entity.Property(e => e.BlobId).HasColumnName("blobId");

                entity.HasOne(d => d.Blob)
                    .WithMany(p => p.ShareItems)
                    .HasForeignKey(d => d.BlobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShareItems_T");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
