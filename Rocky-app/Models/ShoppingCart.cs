using System.ComponentModel.DataAnnotations;

namespace Rocky_app.Models;

public sealed class ShoppingCart
{
    [Key]
    public int Id { get; set; }
}