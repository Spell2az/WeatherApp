using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webapp.Models;

namespace webapp.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration configuration;

    public HomeController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IActionResult Index()
    {
        ViewData["ApiEndpoint"] = configuration["ApiEndpoint"];
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
