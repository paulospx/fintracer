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
