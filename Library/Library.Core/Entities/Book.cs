using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Entities;

public class Book : BaseEntity
{
    [MaxLength(250)]
    public string? Title { get; set; }

    [MaxLength(250)]
    public string? Author { get; set; }

    public int Year { get; set; }

    [MaxLength(13)]
    public string? ISBN { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 100)]
    public int Quantity { get; set; }

    public int GenreId { get; set; }

    public Genre? Genre { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public double RentalPrice { get; set; }
    public ICollection<BorrowRecord>? BorrowRecords { get; set; }
}