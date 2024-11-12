namespace Rocky_app.Models.ViewModels;

public sealed class DetailsViewModel
{
    public required Product Product { get; set; }
    public bool ExistInCart { get; set; }
}