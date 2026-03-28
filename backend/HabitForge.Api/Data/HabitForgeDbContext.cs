using HabitForge.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HabitForge.Api.Data;

public class HabitForgeDbContext : DbContext
{
    public HabitForgeDbContext(DbContextOptions<HabitForgeDbContext> options) : base(options) { }

    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<User> Users => Set<User>();
    public DbSet<HabitCompletion> HabitCompletions => Set<HabitCompletion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Habit>(entity =>
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Name).IsRequired().HasMaxLength(200);
            entity.Property(h => h.Description).HasMaxLength(1000);
            entity.HasOne(h => h.User)
                  .WithMany(u => u.Habits)
                  .HasForeignKey(h => h.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HabitCompletion>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { c.HabitId, c.CompletedDate }).IsUnique();
            entity.HasOne(c => c.Habit)
                  .WithMany(h => h.Completions)
                  .HasForeignKey(c => c.HabitId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.DisplayName).IsRequired().HasMaxLength(100);
        });
    }
}
