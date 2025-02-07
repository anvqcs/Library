using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Created { get; set; }

    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
}