using BBLibraryApp.Data;
using BBLibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    public class InstrumentsController : Controller
    {
        private readonly BBLibraryContext _context;
        public InstrumentsController(BBLibraryContext context) => _context = context;

        // GET: Instruments
        public async Task<IActionResult> Index()
        {
            var listOfInstruments = await _context.Instruments
                .OrderBy(i => i.ScoreOrder)
                .AsNoTracking()
                .ToListAsync();

            var nextScoreOrderNumber = listOfInstruments.Count + 1;

            ViewBag.NextScoreOrderNumber = nextScoreOrderNumber;
            return View(listOfInstruments);
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instruments
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create(int nextScoreOrderNumber)
        {
            ViewBag.NextScoreOrderNumber = nextScoreOrderNumber;
            return View();
        }
        // POST: Instruments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentName,ScoreOrder,IsInDefaultSet")]
        Instrument instrument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instrument);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instruments
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instrument == null)
            {
                return NotFound();
            }
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentToUpdate = await _context.Instruments
                .SingleOrDefaultAsync(i => i.ID == id);

            if (await TryUpdateModelAsync<Instrument>(
                instrumentToUpdate,
                "",
                i => i.InstrumentName, i => i.ScoreOrder, i => i.IsInDefaultSet))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instrumentToUpdate);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instruments
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instruments.SingleOrDefaultAsync(m => m.ID == id);
            _context.Instruments.Remove(instrument);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
