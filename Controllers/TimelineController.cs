using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTracer.Models;

namespace FinTracer.Controllers
{
    public class TimelineController : Controller
    {
        private readonly FinTraceContext _context;

        public TimelineController(FinTraceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Timelines.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }

        public async Task<IActionResult> Json()
        {
            return Json(await _context.Timelines.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeline = await _context.Timelines
                .FirstOrDefaultAsync(m => m.TimeLineId == id);
            if (timeline == null)
            {
                return NotFound();
            }

            return View(timeline);
        }

        // GET: Timeline/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeLineId,CreatedAt,Username,Title,SubTitle,Category,Source,Series,XAxis,YAxis,Tooltip,Notes,ChartType,Settings,Enabled")] Timeline timeline)
        {
            if (ModelState.IsValid)
            {
                timeline.TimeLineId = Guid.NewGuid();
                _context.Add(timeline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeline);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJ([Bind("TimeLineId,CreatedAt,Username,Title,SubTitle,Category,Source,Series,XAxis,YAxis,Tooltip,Notes,ChartType,Settings,Enabled")] Timeline timeline)
        {
            if (ModelState.IsValid)
            {
                timeline.TimeLineId = Guid.NewGuid();
                _context.Add(timeline);
                await _context.SaveChangesAsync();
                return Json(timeline);
            }
            return Json(timeline);
        }


        // GET: Timeline/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeline = await _context.Timelines.FindAsync(id);
            if (timeline == null)
            {
                return NotFound();
            }
            return View(timeline);
        }

        // POST: Timeline/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TimeLineId,CreatedAt,Username,Title,Notes,ChartType,Settings,Enabled")] Timeline timeline)
        {
            if (id != timeline.TimeLineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimelineExists(timeline.TimeLineId))
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
            return View(timeline);
        }

        // GET: Timeline/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeline = await _context.Timelines
                .FirstOrDefaultAsync(m => m.TimeLineId == id);
            if (timeline == null)
            {
                return NotFound();
            }

            return View(timeline);
        }

        // POST: Timeline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var timeline = await _context.Timelines.FindAsync(id);
            if (timeline != null)
            {
                _context.Timelines.Remove(timeline);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimelineExists(Guid id)
        {
            return _context.Timelines.Any(e => e.TimeLineId == id);
        }
    }
}
