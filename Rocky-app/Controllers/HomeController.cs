using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky_app.Data;
using Rocky_app.Models;
using Rocky_app.Models.ViewModels;
using Rocky_app.Utils;
using Rocky_app.Utils.Extensions;

namespace Rocky_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var homeViewModel = new HomeViewModel()
        {
            Categories = await _context.Categories.ToListAsync(),
            Products = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ApplicationType)
                .ToListAsync()
        };
        return View(homeViewModel);
    }

    public async Task<IActionResult> ViewDetails(int id)
    {
        DetailsViewModel detailsViewModel = new()
        {
            Product = await _context.Products.Include(x => x.ApplicationType)
                                             .Include(x => x.Category)
                                             .FirstOrDefaultAsync(x => x.Id == id)
        };

        if (detailsViewModel.Product != null)
        {
            var shoppingCartsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            if (shoppingCartsFromSession != null && shoppingCartsFromSession.Any())
            {
                if (shoppingCartsFromSession.FirstOrDefault(x => x.Id == id) != null)
                {
                    detailsViewModel.ExistInCart = true;
                }
            }
            return View(detailsViewModel);
        }

        return NotFound();
    }
    
    [HttpPost, ActionName("Details")]
    public async Task<IActionResult> DetailsPost(int id)
    {
        List<ShoppingCart> shoppingCarts = new();
        var shoppingCartsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        if (shoppingCartsFromSession != null && shoppingCartsFromSession.Any())
        {
            shoppingCarts = shoppingCartsFromSession;
        }

        shoppingCarts.Add(new ShoppingCart() { Id = id });
        HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("RemoveFromCart")]
    public async Task<IActionResult> RemoveFromCartPost(int id)
    {
        var shoppingCartsFromSession = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
        if (shoppingCartsFromSession != null && shoppingCartsFromSession.Any())
        {
            var shoppingCartToDelete = shoppingCartsFromSession.FirstOrDefault(x => x.Id == id);
            if (shoppingCartToDelete != null) shoppingCartsFromSession.Remove(shoppingCartToDelete);
            HttpContext.Session.Set(WC.SessionCart, shoppingCartsFromSession);
        }
        return RedirectToAction(nameof(ViewDetails), new { id = id });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}