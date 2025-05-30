using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinTracer.Models;

namespace FinTracer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FinTraceContext _context;

    public HomeController(ILogger<HomeController> logger, FinTraceContext context)
    {
        _logger = logger;
        _context = context;
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

    public IActionResult Compare()
    {
        return View();
    }

    public IActionResult Category()
    {
        return View();
    }

    public  IActionResult CompareCurves(string period="202503", string filename1= "C:\\Repos\\Data\\Curves\\Book_2.xlsx", string filename2= "C:\\Repos\\Data\\Curves\\Book_3.xlsx", string sheetname= "AC ZC YC")
    {
        var curvesNames1 = ExcelManager.ReadFirstRow(filename1, sheetname);
        var curvesNames2 = ExcelManager.ReadFirstRow(filename2, sheetname);

        var maturities = curvesNames1.Intersect(curvesNames2).ToList();

        maturities.Remove(string.Empty);
        maturities.Remove("Currency");

        var curves1 = ExcelManager.GetColumnsByHeaders(filename1, string.Join(",", maturities), sheetname);
        var curves2 = ExcelManager.GetColumnsByHeaders(filename2, string.Join(",", maturities), sheetname);

        var result = new List<CompareModel>();
        foreach (var curve1 in curves1)
        {
            foreach(var curve2 in curves2)
            {
                if (curve1.Name == curve2.Name)
                {
                    var deltas = ExcelManager.CalculateDeltas($"Delta-{curve1.Name}-{curve2.Name}",filename1, filename2, curve1, curve2,maturities, period);
                    result.AddRange(deltas);
                }
            }
        }

        _context.Comparisons.AddRange(result);
        _context.SaveChanges();
        return Json(result);
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
