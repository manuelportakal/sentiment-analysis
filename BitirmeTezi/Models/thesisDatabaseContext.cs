using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BitirmeTezi.Models
{
    public partial class thesisDatabaseContext : DbContext
    {
        public thesisDatabaseContext()
        {
        }

        public thesisDatabaseContext(DbContextOptions<thesisDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Emoji> Emojis { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=bitirmetezi.database.windows.net;Initial Catalog=thesisDatabase;User ID=yavuz;Password=Bitirmetezi123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Emoji>(entity =>
            {
                entity.ToTable("emoji");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AngerPoint)
                    .HasMaxLength(255)
                    .HasColumnName("anger_point");

                entity.Property(e => e.Codepoints)
                    .HasMaxLength(255)
                    .HasColumnName("codepoints");

                entity.Property(e => e.FearPoint)
                    .HasMaxLength(255)
                    .HasColumnName("fear_point");

                entity.Property(e => e.Grup)
                    .HasMaxLength(255)
                    .HasColumnName("grup");

                entity.Property(e => e.JoyPoint)
                    .HasMaxLength(255)
                    .HasColumnName("joy_point");

                entity.Property(e => e.LovePoint)
                    .HasMaxLength(255)
                    .HasColumnName("love_point");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.SadnessPoint)
                    .HasMaxLength(255)
                    .HasColumnName("sadness_point");

                entity.Property(e => e.SurprisePoint)
                    .HasMaxLength(255)
                    .HasColumnName("surprise_point");

                entity.Property(e => e.TemelGrup)
                    .HasMaxLength(255)
                    .HasColumnName("temel_grup");

                entity.Property(e => e.Emo)
                    .HasMaxLength(255)
                    .HasColumnName("emo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
