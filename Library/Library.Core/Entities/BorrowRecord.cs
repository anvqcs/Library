using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Entities;

public class BorrowRecord : BaseEntity
{
    [Required]
    public int BookId { get; set; }

    public Book? Book { get; set; }

    [Required]
    public string? ApplicationUserId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime BorrowDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReturnDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public double? RentalCost { get; set; }

}