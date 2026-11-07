using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesCatalog.DAL.Entities;

namespace MoviesCatalog.DAL.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Producer> Producers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .Property(u => u.DateOfBirth)
            .HasColumnType("timestamp without time zone");

        builder.Entity<Movie>()
            .HasIndex(m => m.Title)
            .IsUnique();

        builder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();

        builder.Entity<Producer>()
            .HasIndex(p => p.Name)
            .IsUnique();

        builder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));
    }
}