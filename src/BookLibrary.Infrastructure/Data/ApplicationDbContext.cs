using BookLibrary.Application.Common.Interfaces;
using BookLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.LastName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.Type)
            .HasMaxLength(50);

        modelBuilder.Entity<Book>()
            .Property(b => b.ISBN)
            .HasMaxLength(20);

        modelBuilder.Entity<Book>()
            .Property(b => b.Category)
            .HasMaxLength(100);
    }
}
