using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky_app.Data;
using Rocky_app.Models;
using Rocky_app.Models.ViewModels;
using Rocky_app.Utils;

namespace Rocky_app.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var products = await _db.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.ApplicationType)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Upsert(int id)
    {
        var productViewModel = new ProductViewModel();

        if (id != 0)
        {
            var product = await _db.Products
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.ApplicationType)
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            productViewModel.Product = product;
        }

        var categories = await _db.Categories.ToListAsync();
        var applicationTypes = await _db.ApplicationTypes.ToListAsync();

        productViewModel.ApplicationTypeSelectList = applicationTypes.Select(x => new SelectListItem()
            { Value = x.Id.ToString(), Text = x.ApplicationTypeName });
        productViewModel.CategorySelectList = categories.Select(x => new SelectListItem()
            { Value = x.Id.ToString(), Text = x.CategoryName });


        return View(productViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductViewModel productViewModel)
    {
        var files = HttpContext.Request.Form.Files;
        string fileFilderPath = _env.WebRootPath;

        if (productViewModel.Product.Id == 0)
        {
            string uploadPath = fileFilderPath + WC.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            productViewModel.Product.ImageUrl = fileName + extension;
            _db.Products.Add(productViewModel.Product);
            await _db.SaveChangesAsync();
        }
        else
        {
            //Updating
            var entityToUpdate = await _db.Products.FindAsync(productViewModel.Product.Id);
            if (entityToUpdate != null)
            {
                if (files.Count > 0)
                {
                    string uploadPath = fileFilderPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var oldFile = Path.Combine(uploadPath, entityToUpdate.ImageUrl);
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    using (var fileStream =
                           new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = fileName + extension;
                }

                productViewModel.Product.ImageUrl = entityToUpdate.ImageUrl;
                _db.Entry(entityToUpdate).CurrentValues.SetValues(productViewModel.Product);
                await _db.SaveChangesAsync();
            }
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0) return NotFound();

        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (product != null) return View(product);

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        if (id == null) return BadRequest();

        var productToDelete = await _db.Products.FindAsync(id);
        if (productToDelete == null) return NotFound();

        string fileFolderPath = _env.WebRootPath;
        string uploadPath = fileFolderPath + WC.ImagePath;
        var oldFile = Path.Combine(uploadPath, productToDelete.ImageUrl);

        if (System.IO.File.Exists(oldFile))
        {
            System.IO.File.Delete(oldFile);
        }

        _db.Products.Remove(productToDelete);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}