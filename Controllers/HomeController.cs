using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myGHrepo.Models;
using System.Security.Cryptography;

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
        // Insecure random number generation
        var token = new Random().Next().ToString();

        // Security-sensitive sink: FormsAuthenticationTicket
        var ticket = new FormsAuthenticationTicket(
                1,                      // version
                "user@example.com",     // username
                DateTime.Now,           // issue date
                DateTime.Now.AddMinutes(30), // expiration
                false,                  // persistent
                token                   // user data (contains insecure token)
        );

        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
        Response.Cookies.Add(new System.Web.HttpCookie(
                FormsAuthentication.FormsCookieName, encryptedTicket));



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
