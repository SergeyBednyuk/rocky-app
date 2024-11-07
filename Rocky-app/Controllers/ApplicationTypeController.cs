using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky_app.Data;
using Rocky_app.Models;

namespace Rocky_app.Controllers;

public class ApplicationTypeController : Controller
{
    private readonly AppDbContext _db;

    public ApplicationTypeController(AppDbContext db)
    {
        _db = db;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var applicationTypes = await _db.ApplicationTypes
                                                    .AsNoTracking()
                                                    .ToListAsync();
        return View(applicationTypes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ApplicationType applicationType)
    {
        _db.ApplicationTypes.Add(applicationType);
        await _db.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}