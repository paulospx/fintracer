using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinTracer.Models;

namespace FinTracer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Scan(string path)
    {
        SearchManager.IndexFile(path);
        return Json("Done");
    }

    public IActionResult File()
    {
        return View();
    }

    public IActionResult Query(string q)
    {
        var result = SearchManager.ConstructQuery(q);
        return Json(result);
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
