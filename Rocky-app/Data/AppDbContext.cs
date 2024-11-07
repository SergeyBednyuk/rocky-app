using Microsoft.EntityFrameworkCore;
using Rocky_app.Models;

namespace Rocky_app.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<ApplicationType> ApplicationTypes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}