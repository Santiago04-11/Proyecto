using GtnApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GtnApi.Core.Data;

public class GtnDbContext : DbContext
{
    public GtnDbContext(DbContextOptions<GtnDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(e =>
        {
            e.ToTable("Projects");
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Note>(e =>
        {
            e.ToTable("Notes");
            e.Property(n => n.Title).IsRequired().HasMaxLength(200);
            e.Property(n => n.Content).IsRequired();

            // Al borrar un proyecto, la nota queda sin proyecto (no se borra).
            e.HasOne(n => n.Project)
                .WithMany(p => p.Notes)
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TaskItem>(e =>
        {
            e.ToTable("Tasks");
            e.Property(t => t.Title).IsRequired().HasMaxLength(200);
            e.Property(t => t.Description).HasMaxLength(2000);
            e.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);

            // Al borrar un proyecto, la tarea queda sin proyecto (no se borra).
            e.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(t => new { t.ProjectId, t.Status });
        });
    }
}
