using System.ComponentModel.DataAnnotations;

namespace Rocky_app.Models;

public sealed class Category
{
    [Key]
    public int Id { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}