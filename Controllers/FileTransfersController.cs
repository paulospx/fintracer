using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinTracer.Models;

namespace FinTracer.Controllers
{
    public class FileTransfersController : Controller
    {
        private readonly FinTraceContext _context;

        public FileTransfersController(FinTraceContext context)
        {
            _context = context;
        }

        // GET: FileTransfers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Filetransfers.ToListAsync());
        }

        // GET: FileTransfers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileTransfer = await _context.Filetransfers
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (fileTransfer == null)
            {
                return NotFound();
            }

            return View(fileTransfer);
        }


        public async Task<IActionResult> Copy(string category = "")
        {
            var filesTransfers = _context.Filetransfers.Where(t => t.Category == category).ToList();
            foreach(var transfer in filesTransfers)
            {
                var updatedTransfer = await FileManager.CopyFileAsync(transfer);
                _context.Filetransfers.Update(updatedTransfer);
            }
            _context.SaveChanges();

            return Json("Done");
        }


        public IActionResult Check(string category = "")
        {
            var filesTransfers = _context.Filetransfers.Where(t => t.Category == category).ToList();
            foreach (var transfer in filesTransfers)
            {
                if (System.IO.File.Exists(transfer.SourceFile))
                {
                    transfer.Status = "Integrity Check";
                }
                else
                {
                    transfer.LogMessages = $"The file {transfer.SourceFile} is non existent.";
                    transfer.Status = "Validation";
                }
                _context.Filetransfers.Update(transfer);
            }
            _context.SaveChanges();

            return Json("Done");
        }


        // GET: FileTransfers/Create
        public IActionResult Create()
        {
            var transfer = new FileTransfer()
            {
                SourceCreatedAt = DateTime.Now,
                TargetCreatedAt = DateTime.Now,
                SourceFile = string.Empty,
                TargetFile = string.Empty,
                SourceSize = 0,
                TargetSize = 0,
                LastCopied = DateTime.Now,
                Status = "New"
            };

            return View(transfer);
        }

        // POST: FileTransfers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferId,SourceFile,SourceCreatedAt,SourceSize,SourceMd5,TargetFile,TargetCreatedAt,TargetSize,TargetMd5,LastCopied,IsSelected,Status,ScheduledToCopy,Category,LogMessages")] FileTransfer fileTransfer)
        {
            if (ModelState.IsValid)
            {
                fileTransfer.TransferId = Guid.NewGuid();
                _context.Add(fileTransfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fileTransfer);
        }

        // GET: FileTransfers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileTransfer = await _context.Filetransfers.FindAsync(id);
            if (fileTransfer == null)
            {
                return NotFound();
            }
            return View(fileTransfer);
        }




        // POST: FileTransfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TransferId,SourceFile,SourceCreatedAt,SourceSize,SourceMd5,TargetFile,TargetCreatedAt,TargetSize,TargetMd5,LastCopied,IsSelected,Status,ScheduledToCopy")] FileTransfer fileTransfer)
        {
            if (id != fileTransfer.TransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileTransferExists(fileTransfer.TransferId))
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
            return View(fileTransfer);
        }

        // GET: FileTransfers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileTransfer = await _context.Filetransfers
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (fileTransfer == null)
            {
                return NotFound();
            }

            return View(fileTransfer);
        }

        // POST: FileTransfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fileTransfer = await _context.Filetransfers.FindAsync(id);
            if (fileTransfer != null)
            {
                _context.Filetransfers.Remove(fileTransfer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileTransferExists(Guid id)
        {
            return _context.Filetransfers.Any(e => e.TransferId == id);
        }
    }
}
