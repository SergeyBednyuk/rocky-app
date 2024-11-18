using Microsoft.AspNetCore.Identity;

namespace Rocky_app.Models;

public sealed class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}