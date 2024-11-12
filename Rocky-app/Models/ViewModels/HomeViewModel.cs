namespace Rocky_app.Models.ViewModels;

public sealed class HomeViewModel
{
    public required IEnumerable<Product> Products { get; set; }

    public required IEnumerable<Category> Categories { get; set; }
}