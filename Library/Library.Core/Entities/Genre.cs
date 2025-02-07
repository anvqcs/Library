using System.ComponentModel.DataAnnotations;

namespace Library.Core.Entities;

public class Genre : BaseEntity
{
    [MaxLength(250)] 
    public string? Name { get; set; }

    public ICollection<Book>? Books { get; set; }
}