using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BBLibraryApp.Data;
using BBLibraryApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BBLibraryApp.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    public class VenuesController : Controller
    {
        private readonly BBLibraryContext _context;

        public VenuesController(BBLibraryContext context)
        {
            _context = context;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.AsNoTracking().ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueName,City,Contact,Comments")] Venue venue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(venue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception /*ex*/)
            {
                //Log the error (unkomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.ID == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venueToUpdate = await _context.Venues
                .SingleOrDefaultAsync(v => v.ID == id);

            if (await TryUpdateModelAsync<Venue>(
                venueToUpdate,
                "",
                v => v.VenueName, v => v.City, v => v.Comments, v => v.Contact))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venueToUpdate);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.SingleOrDefaultAsync(v=>v.ID==id);
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
