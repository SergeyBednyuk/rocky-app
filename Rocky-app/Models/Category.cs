using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky_app.Models;

public sealed class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string CategoryName { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid value")]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}