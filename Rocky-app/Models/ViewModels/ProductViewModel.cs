using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky_app.Models.ViewModels;

public sealed class ProductViewModel
{
    public Product Product { get; set; } = new Product();
    public IEnumerable<SelectListItem>? CategorySelectList { get; set; } = Enumerable.Empty<SelectListItem>();

    public IEnumerable<SelectListItem>? ApplicationTypeSelectList { get; set; } = Enumerable.Empty<SelectListItem>();
}