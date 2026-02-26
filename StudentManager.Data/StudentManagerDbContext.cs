using Microsoft.EntityFrameworkCore;
using StudentManager.Common.Contants;
using StudentManager.Data.Configurations;
using StudentManager.Data.Entities;
using System;

namespace StudentManager.Data
{
    public class StudentManagerDbContext : DbContext
    {
        //=== Step 1: Contructor ===//
        // Constructor dùng cho DI
        public StudentManagerDbContext(DbContextOptions<StudentManagerDbContext> options)
            : base(options)
        {
        }

        //=== Step 2: KHai báo DbSet ===//
        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }

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
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentManagerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StudentClassConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());

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
            modelBuilder.Entity<Major>(entity =>
            {
                //entity.ToTable("Majors");
                //entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(MaxLengths.TITLE);
                entity.Property(e => e.Description)
                      .HasMaxLength(MaxLengths.DESCRIPTION);
                entity.Property(e => e.MajorCode)
                      .HasMaxLength(15);
            });

        }
    }
}
