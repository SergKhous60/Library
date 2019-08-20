using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using BBLibraryApp.Data;
using BBLibraryApp.Models;
using BBLibraryApp.Infrastructure;
using BBLibraryApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BBLibraryApp.Controllers
{
    public class PerformancesController : Controller
    {
        private readonly BBLibraryContext _context;
        private readonly Cart cart;

        public PerformancesController(BBLibraryContext context, Cart cartService)
        {
            _context = context;
            cart = cartService;
        }

            [Authorize(Policy ="MembersOnly")]
        // GET: Performances
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int page = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["Date"] = String.IsNullOrEmpty(sortOrder) ? "Date" : "";
            ViewData["Name"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["Venue"] = sortOrder == "Venue" ? "venue_desc" : "Venue";

            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var performances = from p in _context.Performances
                               select p;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                performances = performances.Where(p => p.Name.Contains(searchString) ||
                 p.Date.ToString().Contains(searchString) || p.Venue.VenueName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Name":
                    performances = performances.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    performances = performances.OrderByDescending(p => p.Name);
                    break;
                case "Venue":
                    performances = performances.OrderBy(p => p.Venue.VenueName);
                    break;
                case "venue_desc":
                    performances = performances.OrderByDescending(p => p.Venue.VenueName);
                    break;
                
                case "Date":
                    performances = performances.OrderBy(p => p.Date);
                    break;
                default:
                    performances = performances.OrderByDescending(P => P.Date);
                    break;
            }
            int pageSize = 20;
            ViewBag.Performances = performances.Count();

            return View(await performances.AsNoTracking().Include(p => p.Venue).GetPagedAsync(page, pageSize));
        }
        [Authorize(Policy ="MembersOnly")]
        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Performance with Venue info and instantianing Charts list.
            var viewModel = new PerformanceDetailsData
            {
                Performance = await _context.Performances
                .Include(p => p.Venue)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ID == id),
                Charts = new List<ChartViewModel>()
        };
            // Get the all PerformanceChart info and group them (Distinct) by Chart.ID
            var performCharts = DistinctCharts((int)id);

            // To fill the Charts list wiht individual charts info for the performance
            foreach (var item in performCharts)
            {
                var chartViewModel = new ChartViewModel
                {
                    Chart = item.Chart,
                    ChartListOrder = item.ChartListOrder
                };
                viewModel.Charts.Add(chartViewModel);
            }
             // Counting total seconds for all charts in the performance  
            viewModel.TotalSecs = viewModel.Charts.Sum(i => i.Chart.Minutes * 60 + i.Chart.Seconds);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        [Authorize(Policy = "AdministratorOnly")]
        [HttpPost]
        public async Task<IActionResult> AddCharts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceToUpdate = await _context.Performances
                .Include(p => p.ConcertProgramme)
                    .SingleOrDefaultAsync(p => p.ID == id);

            var chartList = new HashSet<int>(performanceToUpdate.ConcertProgramme.Select(c => c.ChartID));


            foreach (var item in cart.ChartsPool)
            {
                if (!chartList.Contains(item.ID))
                {
                    var newPerfChart = new PerformanceChart
                    {
                        PerformanceID = (int)id,
                        ChartID = item.ID
                    };
                    performanceToUpdate.ConcertProgramme.Add(newPerfChart);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = performanceToUpdate.ID });
        }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Performances/Create
        public IActionResult Create()
        {
            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "VenueName");
            return View();
        }

        [Authorize(Policy = "AdministratorOnly")]
        // POST: Performances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Date,Comments,VenueID")] Performance performance)
        {
            try
            {
                if (ModelState.IsValid)
                    _context.Add(performance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = performance.ID });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Make sure your Performance Name is " +
                    "not a double entry on the same date." +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            ViewData["VenueID"] = new SelectList(_context.Venues, "ID", "VenueName", performance.VenueID);

            return View(performance);
        }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Performances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances
                .Include(p => p.Venue)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ID == id);

            if (performance == null)
            {
                return NotFound();
            }
            PopulatePerformanceChartList(performance, "");
            PopulateVenueDropDownList(performance.VenueID);

            return View(performance);
        }

        [Authorize(Policy = "AdministratorOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCharts)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceToUpdate = await _context.Performances
                .Include(p => p.Venue)
                .Include(p => p.ConcertProgramme)
                    .ThenInclude(pc => pc.Chart)
                    .SingleOrDefaultAsync(p => p.ID == id);

            if (await TryUpdateModelAsync<Performance>(
                performanceToUpdate,
                "",
                p => p.Name, p => p.Date, p => p.Comments, p => p.VenueID, p => p.ConcertProgramme))
            {
                UpdateChartList(selectedCharts, performanceToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = performanceToUpdate.ID });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                }
            }

            UpdateChartList(selectedCharts, performanceToUpdate);
            PopulatePerformanceChartList(performanceToUpdate, "");
            return View(performanceToUpdate);
        }
        [Authorize(Policy ="MembersOnly")]
        [HttpGet]
        public async Task<IActionResult> ShowArtistsList(int id, int performanceId)
        {
            var chart = await GetChart(id);

            if (chart.Instrumentation.Count == 0)
            {
                return RedirectToAction(nameof(ChartsController.Edit), "Charts", new { id = chart.ID });
            }
            
            return View("ArtistsList", await PopulateArtistList(chart, performanceId));
        }

        [Authorize(Policy = "AdministratorOnly")]
        [HttpGet]
        public async Task<IActionResult> EditArtistsList(int id, int performanceId)
        {
            var chart = await GetChart(id);

            var people = from p in _context.People
                       orderby p.LastName
                       select p;

            var viewModel = await PopulateArtistList(chart, performanceId);
            viewModel.People = people.AsNoTracking().ToList();

                return View("EditArtistsList", viewModel);
        }

        [Authorize(Policy = "AdministratorOnly")]
        [HttpPost, ActionName("EditArtistsList")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArtistsListPost(int id, int performanceId)
        {
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var personInstrumentsToUpdate = await _context.PerformanceCharts
                .Where(p => p.PerformanceID == performanceId)
                .Where(p => p.ChartID == id)
                .AsNoTracking()
                .ToListAsync();

            var chartToUpdate = await _context.Charts
                .Include(c => c.Instrumentation)
                .ThenInclude(i => i.Instrument)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.ID == id);

            var personInstruments = chartToUpdate.Instrumentation.Select(i => i.Instrument)
                .OrderBy(i => i.ScoreOrder);

                if (personInstrumentsToUpdate.Count() == 1 && personInstrumentsToUpdate[0].Instrument == null)
                {
                    foreach (var instrument in personInstruments)
                    {
                        if (personInstrumentsToUpdate[0].InstrumentID == null)
                        {
                            personInstrumentsToUpdate[0].InstrumentID = instrument.ID;
                            personInstrumentsToUpdate[0].PersonID = 
                                int.Parse(Request.Form[$"PersonID_{instrument.ID}"]);
                        _context.Entry<PerformanceChart>(personInstrumentsToUpdate[0])
                            .State = EntityState.Modified;
                    }
                    else
                        {
                            var perInst = new PerformanceChart
                            {
                                PerformanceID = performanceId,
                                ChartID = id,
                                InstrumentID = instrument.ID,
                                PersonID = int.Parse(Request.Form[$"PersonID_{instrument.ID}"]),
                                ChartListOrder = personInstrumentsToUpdate[0].ChartListOrder
                            };
                        _context.Entry<PerformanceChart>(perInst).State = EntityState.Added;
                        personInstrumentsToUpdate.Add(perInst);
                        }
                    }
                }
                else
                    {
                        foreach (var instrument in personInstruments)
                        {
                            foreach (var item in personInstrumentsToUpdate)
                            {
                                if (instrument.ID == item.InstrumentID)
                                    {
                                        item.PersonID = int.Parse(Request.Form[$"PersonID_{instrument.ID}"]);
                                        _context.Entry<PerformanceChart>(item)
                                .State = EntityState.Modified;
                                    }
                            }
                        }
                    }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                Console.WriteLine(new Exception(ex.Message.ToString(), ex.InnerException));

                var instrumentPersonInfo = new InstrumentPersonInfo
                {
                    Chart = chartToUpdate,
                    PersonInstruments = personInstruments,
                    PerformanceID = performanceId
                };

                var pers = from p in _context.People
                           orderby p.LastName
                           select p;

                ViewData["Artists"] = new SelectList(pers.AsNoTracking(), "ID", "FullName");
                return View(instrumentPersonInfo);
            }
                return RedirectToAction(nameof(ShowArtistsList),
                    new { id = id, performanceId = performanceId });
            }

        [Authorize(Policy = "AdministratorOnly")]
        // GET: Performances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performances
                .Include(p => p.Venue)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        [Authorize(Policy = "AdministratorOnly")]
        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performance = await _context.Performances.FindAsync(id);
            _context.Performances.Remove(performance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulatePerformanceChartList(Performance performance, string chartSortingOrder)
        {
            var allCharts = DistinctCharts(performance.ID);

            var performanceChartList = new HashSet<int>(allCharts.Select(i => i.ChartID));
            var viewModel = new List<ChartList>();

            foreach (var perfChart in allCharts)
            {
                viewModel.Add(new ChartList
                {
                    ChartID = perfChart.ChartID,
                    Name = perfChart.Chart.ChartName,
                    Assigned = performanceChartList.Contains(perfChart.ChartID),
                    ListOrder = ((allCharts
                    .SingleOrDefault(p => p.ChartID == perfChart.ChartID))?.ChartListOrder)
                });
            }
            if (chartSortingOrder == "Name")
            {
                ViewBag.PerformanceChartList = viewModel.OrderBy(p => p.Name).ToList();
            }
            else
            {
                ViewBag.PerformanceChartList = viewModel.OrderBy(p => p.ListOrder).ToList();
            }
        }

        private void PopulateVenueDropDownList(object selectedVenue = null)
        {
            var venueQuery = from v in _context.Venues
                             orderby v.VenueName
                             select v;
            ViewData["VenueID"] = new SelectList(venueQuery.AsNoTracking(), "ID", "VenueName", selectedVenue);
        }

        private void UpdateChartList(string[] selectedCharts, Performance performanceToUpdate)
        {
            if (selectedCharts == null)
            {
                performanceToUpdate.ConcertProgramme = new List<PerformanceChart>();
                return;
            }

            var selectedChartsHS = new HashSet<string>(selectedCharts);
            var chartList = new HashSet<int>(performanceToUpdate.ConcertProgramme.Select(c => c.ChartID));

            foreach (var chart in _context.Charts)
            {
                if (selectedChartsHS.Contains(chart.ID.ToString()))
                {
                    if (!chartList.Contains(chart.ID))
                    {
                        performanceToUpdate.ConcertProgramme.Add(new PerformanceChart
                        {
                            PerformanceID = performanceToUpdate.ID,
                            ChartID = chart.ID,
                        });
                    }
                }
                else
                {
                    if (chartList.Contains(chart.ID))
                    {
                        var chartPerfToRemove =
                            performanceToUpdate.ConcertProgramme
                            .Where(p => p.ChartID == chart.ID);
                        _context.RemoveRange(chartPerfToRemove);
                    }
                }
            }

            var performanceChartList = performanceToUpdate.ConcertProgramme;
            var valueFromTheForm = "";
            foreach (var item in performanceChartList)
            {
                item.ChartListOrder = int.Parse(valueFromTheForm =
                    Request.Form[$"chartListOrder {item.ChartID}"].ToString() == "" ? "1"
                    : Request.Form[$"chartListOrder {item.ChartID}"].ToString());
            }
        }

        private async Task<ArtistsListInfo> PopulateArtistList(Chart chart,int performanceId)
        {
            var person = await _context.People
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.LastName == "<None>");

            var instrumentPeople = new List<InstrumentPerson>();

            var chartWithInstrumentAndPerson = new List<PerformanceChart>();

            var chartInProgramme = await _context.PerformanceCharts
                .Where(pc => pc.PerformanceID == performanceId)
                .Where(pc => pc.ChartID == chart.ID)
                .Include(pc => pc.Chart)
                .Include(pc => pc.Instrument)
                .Include(pc => pc.Person)
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in chartInProgramme)
            {
                if (item.Instrument == null)
                {
                    foreach (var chartInstrument in chart.Instrumentation)
                    {
                        var instPerson = new InstrumentPerson
                        {
                            Instrument = chartInstrument.Instrument,
                            Person = person,
                            ScoreOrder = chartInstrument.Instrument.ScoreOrder
                        };
                        instrumentPeople.Add(instPerson);
                    }
                }
                else
                {
                    var instPerson = new InstrumentPerson
                    {
                        Instrument = item.Instrument,
                        Person = item.Person,
                        ScoreOrder = item.Instrument.ScoreOrder
                    };
                    instrumentPeople.Add(instPerson);
                }
            }
            var viewModel = new ArtistsListInfo
            {
                PerformanceID = performanceId,
                Chart = chart,
                InstrumentPeopleInfo = instrumentPeople
            };
            return viewModel;
        }

        private async Task<Chart> GetChart(int id)
        {
            var chart = await _context.Charts
                .Include(c => c.Instrumentation)
                .ThenInclude(i => i.Instrument)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.ID == id);
            return chart;
        }

        private List<PerformanceChart> DistinctCharts(int performanceId)
        {
            var performCharts = _context.PerformanceCharts
                .Where(pc => pc.PerformanceID == performanceId)
                .Include(pc => pc.Chart)
                .GroupBy(c => c.Chart.ID, (key, c) => c.FirstOrDefault())
                .OrderBy(c => c.ChartListOrder)
                .AsNoTracking()
                .ToList();

            return performCharts;
        }

        public async Task<IActionResult> printRepertoire(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Performance with Venue info and instantianing Charts list.
            var viewModel = new PerformanceDetailsData
            {
                Performance = await _context.Performances
                .Include(p => p.Venue)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ID == id),
                Charts = new List<ChartViewModel>()
            };
            // Get the all PerformanceChart info and group them (Distinct) by Chart.ID
            var performCharts = DistinctCharts((int)id);

            // To fill the Charts list wiht individual charts info for the performance
            foreach (var item in performCharts)
            {
                var chartViewModel = new ChartViewModel
                {
                    Chart = item.Chart,
                    ChartListOrder = item.ChartListOrder
                };
                viewModel.Charts.Add(chartViewModel);
            }
            // Counting total seconds for all charts in the performance  
            viewModel.TotalSecs = viewModel.Charts.Sum(i => i.Chart.Minutes * 60 + i.Chart.Seconds);

            if (viewModel == null)
            {
                return NotFound();
            }

            return new ViewAsPdf(
                "printRepertoire",
                viewModel);
        }
    }
}
