using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BBLibraryApp.Data;
using BBLibraryApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BBLibraryApp.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    public class PeopleController : Controller
    {
        private readonly BBLibraryContext _context;

        public PeopleController(BBLibraryContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.People
                .OrderBy(p => p.LastName)
                .AsNoTracking()
                .ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,Address,Email,Phone,Note,IsFullTime")]
        Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(person);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var personToUpdate = await _context.People
                .SingleOrDefaultAsync(p => p.ID == id);
            if (await TryUpdateModelAsync<Person>(
                personToUpdate,
                "",
                p => p.FirstName, p => p.LastName, p => p.Note, p => p.Phone, p => p.Email, p => p.Address,
                p=>p.IsFullTime))
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
            return View(personToUpdate);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.SingleOrDefaultAsync(m => m.ID == id);

            try
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to Delete person. " +
                                "Try again, and if the problem persists, " +
                                "see your system administrator.");
            }
            return View(person);
        }
    }
}
