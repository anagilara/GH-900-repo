using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myGHrepo.Models;

namespace myGHrepo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            _logger.LogInformation("User-Agent: {UserAgent}", userAgent);
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in Index action.");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
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
