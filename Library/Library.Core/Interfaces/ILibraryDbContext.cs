using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Interfaces;

public interface ILibraryDbContext
{
    DbSet<ApplicationUser>? ApplicationUsers { get; }
    DbSet<Book>? Books { get; }
    DbSet<Genre>? Genres { get; }
    DbSet<BorrowRecord>? BorrowRecords { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}