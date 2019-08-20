using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBLibraryApp.Data;
using BBLibraryApp.Models.ViewModels;
using BBLibraryApp.Infrastructure;
using BBLibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace BBLibraryApp.Controllers
{
    [Authorize]
    public class ChartsController : Controller
    {
        private readonly BBLibraryContext _context;

        public ChartsController(BBLibraryContext context)
        {
            _context = context;
        }

        // GET: Charts
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int page=1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ShelfNumber"] = String.IsNullOrEmpty(sortOrder) ? "shelf_desc" : "";
            ViewData["ArrangerSortParam"] = sortOrder == "Arranger" ? "arr_desc" : "Arranger";
            ViewData["Note"] = sortOrder == "Note" ? "note_desc" : "Note";
            ViewData["Time"] = sortOrder == "Time" ? "time_desc" : "Time";
            ViewData["NameSortParam"] = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
;
            var charts = from c in _context.Charts
                         select c;

            int nextShelfNumber;
            if (charts.Count() != 0)
            {
                nextShelfNumber = charts.Max(c => c.ShelfNumber);
            }
            else
            {
                nextShelfNumber = 0;
            }

            ViewBag.NextShelfNumber = nextShelfNumber + 1;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                charts = charts.Where(c => c.ShelfNumber.ToString().Contains(searchString) ||
                  c.ChartName.Contains(searchString) || c.Arranger.Contains(searchString) ||
                  c.Note.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "shelf_desc":
                    charts = charts.OrderByDescending(c => c.ShelfNumber);
                    break;
                case "Arranger":
                    charts = charts.OrderBy(c => c.Arranger);
                    break;
                case "arr_desc":
                    charts = charts.OrderByDescending(c => c.Arranger);
                    break;
                case "Note":
                    charts = charts.OrderBy(c => c.Note);
                    break;
                case "note_desc":
                    charts = charts.OrderByDescending(c => c.Note);
                    break;
                case "Time":
                    charts = charts.OrderBy(c => c.Time);
                    break;
                case "time_desc":
                    charts = charts.OrderByDescending(c => c.Time);
                    break;
                case "Name":
                    charts = charts.OrderBy(c => c.ChartName);
                    break;
                case "name_desc":
                    charts = charts.OrderByDescending(c => c.ChartName);
                    break;
                default:
                    charts = charts.OrderBy(c => c.ShelfNumber);
                    break;
            }
            ViewBag.TotalSecs = charts.Sum(i => i.Minutes * 60 + i.Seconds);
            int pageSize = 100;
            return View(await charts.AsNoTracking().GetPagedAsync(page, pageSize));
        }

        // GET: Charts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new ChartDetailsData
            {
                Chart = await _context.Charts
                .Include(c => c.Instrumentation)
                    .ThenInclude(c => c.Instrument)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id)
            };
            viewModel.Instruments = viewModel.Chart.Instrumentation
                .Select(i => i.Instrument).OrderBy(i => i.ScoreOrder);

            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Charts/Create
        public IActionResult Create(int nextShelfNumber)
        {
            PopulateDefaultInstrumentationList();

            ViewBag.NextShelfNumber = nextShelfNumber;

            return View();
        }

        [Authorize(Policy = "AdministratorOnly")]
        // POST: Charts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChartName,Minutes,Seconds," +
            "Composer,Arranger,RecordingUrl,Note,ShelfNumber")] Chart chart)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var defaultInstruments = Request.Form["ChartInstruments"];
                    chart.Instrumentation = new List<ChartInstrument>();

                    foreach (var item in defaultInstruments)
                    {
                        chart.Instrumentation.Add(new ChartInstrument
                        {
                            ChartID = chart.ID,
                            InstrumentID = int.Parse(item)
                        });
                    }
                    _context.Add(chart);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = chart.ID });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            PopulateDefaultInstrumentationList();

            return View(chart);
        }

        private void PopulateDefaultInstrumentationList()
        {
            var instrumentQuery = from i in _context.Instruments
                                  orderby i.ScoreOrder
                                  select i;

            var defaultInstrumentList = instrumentQuery.AsNoTracking()
                .Where(i => i.IsInDefaultSet)
                .Select(i => i.ID).ToList();

            ViewData["ChartInstruments"] = new MultiSelectList(instrumentQuery.AsNoTracking(),
                "ID", "InstrumentName", defaultInstrumentList);
        }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Charts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chart = await _context.Charts
                .Include(c => c.Instrumentation)
                    .ThenInclude(i => i.Instrument)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.ID == id);

            if (chart == null)
            {
                return NotFound();
            }

            PopulateInstrumentation(chart);
            return View(chart);
        }

        private void PopulateInstrumentation(Chart chart)
        {
            var allInstruments = _context.Instruments;
            var chartInstrumentation = new HashSet<int>(chart.Instrumentation.Select(i => i.InstrumentID));
            var viewModel = new List<InstrumentViewModel>();

            foreach (var instrument in allInstruments)
            {
                viewModel.Add(new InstrumentViewModel
                {
                    InstrumentID = instrument.ID,
                    Name = instrument.InstrumentName,
                    Assigned = chartInstrumentation.Contains(instrument.ID),
                    ScoreOrder = instrument.ScoreOrder
                });
            }

            ViewBag.Instrumentation = viewModel.OrderBy(i => i.ScoreOrder).ToList();
        }

        [Authorize(Policy = "AdministratorOnly")]
        // POST: Charts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedInstruments)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chartToUpdate = await _context.Charts
                .Include(c => c.Instrumentation)
                    .ThenInclude(i => i.Instrument)
                .SingleOrDefaultAsync(c => c.ID == id);

            if (await TryUpdateModelAsync<Chart>(
                chartToUpdate,
                "",
                c => c.ChartName, c => c.Arranger, c => c.Composer, c => c.Minutes, 
                c => c.Seconds, c => c.Note, c => c.RecordingUrl,
                c => c.Instrumentation, c => c.ShelfNumber))
            {
                UpdateInstrumentation(selectedInstruments, chartToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                    // throw new Exception(ex.Message, ex.InnerException);
                }
                return RedirectToAction(nameof(Details), new { id = chartToUpdate.ID });
            }
            UpdateInstrumentation(selectedInstruments, chartToUpdate);
            PopulateInstrumentation(chartToUpdate);
            return View(chartToUpdate);
        }
        private void UpdateInstrumentation(string[] selectedInstruments, Chart chartToUpdate)
        {
            if (selectedInstruments == null)
            {
                chartToUpdate.Instrumentation = new List<ChartInstrument>();
                return;
            }

            var selectedInstrumentsHS = new HashSet<string>(selectedInstruments);
            var instrumentation = new HashSet<int>(chartToUpdate.Instrumentation.Select(i => i.Instrument.ID));
            foreach (var instrument in _context.Instruments)
            {
                if (selectedInstrumentsHS.Contains(instrument.ID.ToString()))
                {
                    if (!instrumentation.Contains(instrument.ID))
                    {
                        chartToUpdate.Instrumentation.Add(new ChartInstrument
                        {
                            ChartID = chartToUpdate.ID,
                            InstrumentID = instrument.ID
                        });
                    }
                }
                else
                {
                    if (instrumentation.Contains(instrument.ID))
                    {
                        ChartInstrument instrumentToRemove =
                            chartToUpdate.Instrumentation.SingleOrDefault(i => i.InstrumentID == instrument.ID);
                        _context.Remove(instrumentToRemove);
                    }
                }
            }
        }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Charts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chart = await _context.Charts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (chart == null)
            {
                return NotFound();
            }
            return View(chart);
        }

        [Authorize(Policy = "AdministratorOnly")]
        // POST: Charts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chart = await _context.Charts.SingleOrDefaultAsync(m => m.ID == id);
            _context.Charts.Remove(chart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}