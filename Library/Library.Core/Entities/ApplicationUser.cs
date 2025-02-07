using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Library.Core.Entities;

public class ApplicationUser : IdentityUser
{
    [Required] [MaxLength(50)] 
    public string? FirstName { get; set; }

    [Required] [MaxLength(50)] 
    public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    [MaxLength(250)] 
    public string? Address { get; set; }

    public bool IsDeleted { get; set; } = false;

    public ICollection<BorrowRecord>? BorrowRecords { get; set; }
}