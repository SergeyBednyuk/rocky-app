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

    public async Task<IActionResult> Edit(int? id)
    {
        if (id > 0)
        {
            var applicationType = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (applicationType == null)
                return NotFound();
            return View(applicationType);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ApplicationType applicationType)
    {
        if (applicationType.Id > 0)
        {
            var applicationTypeToEdit = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == applicationType.Id);
            if (applicationTypeToEdit == null)
            {
                return NotFound();
            }
            _db.Entry(applicationTypeToEdit).CurrentValues.SetValues(applicationType);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return NotFound();
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id > 0)
        {
            var applicationType = await _db.ApplicationTypes
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id);

            if (applicationType == null)
            {
                return NotFound();
            }
            return View(applicationType);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id.HasValue && id > 0)
        {
            var applicationType = await _db.ApplicationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (applicationType == null)
            {
                return NotFound();
            }
            _db.ApplicationTypes.Remove(applicationType);
            await  _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return BadRequest();
    }
}