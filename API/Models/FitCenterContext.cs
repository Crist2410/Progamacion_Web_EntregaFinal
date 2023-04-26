using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class FitCenterContext : DbContext
{
    public FitCenterContext()
    {
    }

    public FitCenterContext(DbContextOptions<FitCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignment { get; set; }

    public virtual DbSet<Class> Class { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)

        {

            IConfigurationRoot configuration = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())

                        .AddJsonFile("appsettings.json")

                        .Build();

            var connectionString = configuration.GetConnectionString("DBFitCenter");

            optionsBuilder.UseMySQL(connectionString);

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("assignment");

            entity.HasIndex(e => e.ClassId, "CLASS_ID");

            entity.HasIndex(e => e.UserId, "USER_ID");

            entity.Property(e => e.AssignmentId).HasColumnName("ASSIGNMENT_ID");
            entity.Property(e => e.AssignmentDate)
                .HasColumnType("date")
                .HasColumnName("ASSIGNMENT_DATE");
            entity.Property(e => e.AssignmentGrade).HasColumnName("ASSIGNMENT_GRADE");
            entity.Property(e => e.ClassId).HasColumnName("CLASS_ID");
            entity.Property(e => e.Status)
                .HasColumnType("enum('PENDING','CONFIRMED','CANCELLED')")
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_ibfk_1");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("class");

            entity.Property(e => e.ClassId).HasColumnName("CLASS_ID");
            entity.Property(e => e.ClassDescription)
                .HasMaxLength(256)
                .HasColumnName("CLASS_DESCRIPTION");
            entity.Property(e => e.ClassName)
                .HasMaxLength(256)
                .HasColumnName("CLASS_NAME");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("END_DATE");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("END_TIME");
            entity.Property(e => e.Location)
                .HasMaxLength(256)
                .HasColumnName("LOCATION");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("START_DATE");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("START_TIME");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("BIRTH_DATE");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FirstName)
                .HasMaxLength(128)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.LastName)
                .HasMaxLength(128)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Role)
                .HasColumnType("enum('ENTRENADOR','ESTUDIANTE')")
                .HasColumnName("ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
