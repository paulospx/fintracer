using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTracer.Models;

namespace FinTracer.Controllers
{
    public class CompareController : Controller
    {
        private readonly FinTraceContext _context;

        public CompareController(FinTraceContext context)
        {
            _context = context;
        }

        // GET: Compare
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comparisons.ToListAsync());
        }

        public async Task<IActionResult> List(string period)
        {
            var result = await _context.Comparisons
                .Where(p => p.Period == period)
                .OrderByDescending(p => p.DeltaSum)
                .ToListAsync();
            return Json(result);
        }


        // GET: Compare/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compareModel = await _context.Comparisons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compareModel == null)
            {
                return NotFound();
            }

            return View(compareModel);
        }

        // GET: Compare/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compare/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Comments,Description,Period,SourceFile,TargetFile,SourceCurve,TargetCurve,Maturities,Delta,CreatedBy,CreatedAt")] CompareModel compareModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compareModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compareModel);
        }

        // GET: Compare/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compareModel = await _context.Comparisons.FindAsync(id);
            if (compareModel == null)
            {
                return NotFound();
            }
            return View(compareModel);
        }

        // POST: Compare/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Comments,Description,Period,SourceFile,TargetFile,SourceCurve,TargetCurve,Maturities,Delta,CreatedBy,CreatedAt")] CompareModel compareModel)
        {
            if (id != compareModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compareModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompareModelExists(compareModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(compareModel);
        }

        // GET: Compare/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compareModel = await _context.Comparisons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compareModel == null)
            {
                return NotFound();
            }

            return View(compareModel);
        }

        // POST: Compare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compareModel = await _context.Comparisons.FindAsync(id);
            if (compareModel != null)
            {
                _context.Comparisons.Remove(compareModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompareModelExists(int id)
        {
            return _context.Comparisons.Any(e => e.Id == id);
        }
    }
}
