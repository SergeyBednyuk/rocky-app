using Microsoft.EntityFrameworkCore;
using Rocky_app.Models;

namespace Rocky_app.Data;

public sealed class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Sever=LAPTOP-RM76L8L8;Database=rocky-app;Trusted_Connection=True;MultipleActiveResultSets=true");
    }*/
}