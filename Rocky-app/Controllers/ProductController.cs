using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky_app.Data;
using Rocky_app.Models;
using Rocky_app.Models.ViewModels;

namespace Rocky_app.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _db;

    public ProductController(AppDbContext db)
    {
        _db = db;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var products = await _db.Products
                                    .AsNoTracking()
                                    .Include(x => x.Category)
                                    .ToListAsync();
        if (products.Count == 0)
        {
            return View(products);
        }
        
        return View(products);
    }

    public async Task<IActionResult> Upsert(int? id)
    {
        /*IEnumerable<SelectListItem> categories = _db.Categories
                                                    .AsNoTracking()
                                                    .Select(x => new SelectListItem
                                                    {
                                                        Text = x.CategoryName,
                                                        Value = x.Id.ToString()
                                                    });
        
        ViewBag.CategoryDropDown = categories;*/

        var productViewModel = new ProductViewModel()
        {
            CategorySelectList = _db.Categories
                                    .AsNoTracking()
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.CategoryName, Value = x.Id.ToString()
                                    })
        };
        
        if (id != null)
        {
            productViewModel.Product = await _db.Products.FindAsync(id); 
            if (productViewModel.Product == null)
            {
                return NotFound();
            }
        }
        return View(productViewModel);
    }
}