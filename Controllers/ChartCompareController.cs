using Microsoft.AspNetCore.Mvc;

namespace FinTracer.Controllers
{
    public class ChartCompareController : Controller
    {
        private readonly IConfiguration _configuration;      
        public ChartCompareController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult GetFiles()
        {
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;
            var files = ExcelManager.GetFilesWithExtension(_reportingPath, "xlsx");
            return Json(files);
        }


        [HttpGet]
        public IActionResult DownloadFile(string filename)
        {
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;
            string filePath = $"{_reportingPath}\\{filename}";

            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path cannot be null or empty.");
            }

            // Ensure the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                // Get file bytes and name
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var fileName = Path.GetFileName(filePath);

                // Set the MIME type for Excel and CSV
                string mimeType = fileName.EndsWith(".csv") ? "text/csv" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // Return the file for download
                return File(fileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public ActionResult GetColumns(string excel= "Book_3.xlsx")
        {
            //int _limitRowsFrom = 2;
            //int _limitRowsTo = 100;
            //int.TryParse(_configuration["Limit.Rows.From"], out _limitRowsFrom);
            //int.TryParse(_configuration["Limit.Rows.From"], out _limitRowsTo);
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;

            var cols = ExcelManager.ReadFirstRow($"{_reportingPath}\\{excel}");
            return Json(cols);
        }

        public ActionResult GetSerie(string name, string excel = "Book_3.xlsx")
        {
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;
            var serie = ExcelManager.GetColumnsByHeaders($"{_reportingPath}\\{excel}", name);
            return Json(serie);
        }

        public ActionResult GetSeries(string names, string excel = "Book_3.xlsx")
        {
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;
            var series = ExcelManager.GetColumnsByHeaders($"{_reportingPath}\\{excel}",names);

            return Json(series);
        }


        public ActionResult GetRandomSeries()
        {
            var result = ExcelManager.GenerateRandomCurves();
            return Json(result);
        }


        public ActionResult GetCategories()
        {
            string _reportingPath = _configuration["Reporting:DataPath"] ?? string.Empty;
            var cols = ExcelManager.ReadFirstRow($"{_reportingPath}\\Book_3.xlsx");
            return Json(cols);
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Scatter()
        {
            return View();
        }

        public ActionResult Heatmap()
        {
            return View();
        }


        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
