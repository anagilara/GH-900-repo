using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myGHrepo.Models;
//test1
//test PR
//test 4 PR
//test 5 PR
//squash test PR 2
//squash test PR 3
//rebase 2
//rebase 3
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
        return View();
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
