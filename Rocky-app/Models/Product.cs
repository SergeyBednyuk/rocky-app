using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky_app.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, int.MaxValue, ErrorMessage = "Price is required and must be greater than 0.01")]
    public double Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    // relationships keys
    [Display(Name = "Category Type")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]    
    public virtual Category Category { get; set; }

    [Display(Name = "Application Type")]
    public int ApplicationTypeId { get; set; }
    [ForeignKey("ApplicationTypeId")]
    public virtual ApplicationType ApplicationType { get; set; }
}