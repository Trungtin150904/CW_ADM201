using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Common.Contants;
using LinkShortener.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortener.Data
{
    public class StudentManagerIdentityDbContext : IdentityDbContext
    {
        public StudentManagerIdentityDbContext(DbContextOptions<StudentManagerIdentityDbContext> options)
        : base(options)
        { }

        //=== Khai bao DbSet ===//
        public DbSet<StudentManagerUser> StudentManagerUsers { get; set; }
        public DbSet<StudentManagerRole> StudentManagerRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentManagerUser>()
                .Property(p => p.FullName)
                .HasMaxLength(MaxLengths.FULL_NAME);
            modelBuilder.Entity<StudentManagerUser>()
                .Property(p => p.Avatar)
                .HasMaxLength(MaxLengths.AVATAR);

            modelBuilder.Entity<StudentManagerRole>()
                .Property(p => p.Description)
                .HasMaxLength(MaxLengths.DESCRIPTION);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    var connectionString = "Server=10.25.32.169, 1433;Database=TheaterIdentityDbContext; User Id=sa; password=Riolish@12345; TrustServerCertificate=True; MultipleActiveResultSets=true;";
            //    optionsBuilder.UseSqlServer(connectionString);
            //}
        }

    }
}
