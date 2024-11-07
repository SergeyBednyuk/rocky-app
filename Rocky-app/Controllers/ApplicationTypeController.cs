using Microsoft.AspNetCore.Mvc;

namespace Rocky_app.Controllers;

public class ApplicationTypeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}