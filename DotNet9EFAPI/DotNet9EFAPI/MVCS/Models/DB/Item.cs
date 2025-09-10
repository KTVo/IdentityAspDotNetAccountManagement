using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet9EFAPI.MVCS.Models.DB;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // NVARCHAR(MAX) NOT NULL DEFAULT N''
    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Name { get; set; } = string.Empty;

    // NVARCHAR(MAX) NULL
    [Column(TypeName = "nvarchar(max)")]
    public string? Color { get; set; }

    // REAL NOT NULL DEFAULT 0  → C# float (System.Single)
    [Required]
    [Column(TypeName = "real")]
    public float Price { get; set; }

    // INT NULL
    public int? SerialNumberId { get; set; }

    // INT NULL (FK to dbo.Categories(Id))
    public int? CategoryId { get; set; }

    // Optional navigation (requires a Category entity in your model)
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
}
