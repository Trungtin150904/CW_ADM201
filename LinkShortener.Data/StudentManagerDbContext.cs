using Microsoft.EntityFrameworkCore;
using LinkShortener.Common.Contants;
using LinkShortener.Data.Entities;
using System;

namespace LinkShortener.Data
{
    public class LinkShortenerDbContext : DbContext
    {
        //=== Step 1: Contructor ===//
        // Constructor dùng cho DI
        public LinkShortenerDbContext(DbContextOptions<LinkShortenerDbContext> options)
            : base(options)
        {
        }

        //=== Step 2: KHai báo DbSet ===//
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //=== Step 3: OnConfiguring ===//
        // Dùng khi KHÔNG cấu hình trong Program.cs
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(
            //        "Server=localhost;Database=StudentDb;Trusted_Connection=True;TrustServerCertificate=True");
            //}
        }

        //=== Step 4: OnModelCreating ===//
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkShortenerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            // ===== Student =====
            //modelBuilder.Entity<Student>(entity =>
            //{
            //    //entity.ToTable("Students");
            //    //entity.HasKey(e => e.Id);

            //    entity.Property(e => e.FullName)
            //          .IsRequired()
            //          .HasMaxLength(MaxLengths.FULL_NAME);

            //    entity.Property(e => e.Phone)
            //          .HasMaxLength(15);
            //    entity.Property(e => e.Email)
            //          .HasMaxLength(250);
            //    entity.Property(e => e.Address)
            //          .HasMaxLength(500);

            //    //entity.Property(e => e.Age)
            //    //      .HasDefaultValue(18);
            //});

            // ===== Course =====
            

        }
    }
}
