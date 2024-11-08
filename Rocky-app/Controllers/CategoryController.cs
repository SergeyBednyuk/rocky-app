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
    
    //GET - Edit
    public async Task<IActionResult> Edit(int? id)
    {
        if (id != null)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        var categoryToEdit = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
        if (categoryToEdit != null)
        {
            _db.Entry(categoryToEdit).CurrentValues.SetValues(category);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id != null)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return View(category);
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}