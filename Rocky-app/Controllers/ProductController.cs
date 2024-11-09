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
                                    /*.AsNoTracking()*/
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

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + extension), FileMode.Create))
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
}