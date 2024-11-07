using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky_app.Data;
using Rocky_app.Models;

namespace Rocky_app.Controllers;

public class CategoryController : Controller
{
    private readonly AppDbContext _db;

    public CategoryController(AppDbContext db)
    {
        _db = db;
    }
    
    // GET
    public IActionResult Index()
    {
        var categories = _db.Categories
                                        .AsNoTracking()
                                        .ToList();
        return View(categories);
    }

    // GET - Create
    public IActionResult Create()
    {
        return View();
    }
    
    //POST - Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");   
        }
        else
        {
            return View(category);
        }
    }
}