using Library.Core.Entities;
using Library.Core.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data;

public class LibraryDbContext : IdentityDbContext<ApplicationUser>, ILibraryDbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    public DbSet<Book>? Books { get; set; }
    public DbSet<Genre>? Genres { get; set; }
    public DbSet<BorrowRecord>? BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        builder.Entity<Book>().ToTable("Books");
        builder.Entity<Genre>().ToTable("Genres");
        builder.Entity<BorrowRecord>().ToTable("BorrowRecords");
    }
}