using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NominaWeb.Data;
using NominaWeb.Models;

namespace NominaWeb.Controllers
{
    public class TitlesController : Controller
    {
        private readonly NominaDbContext _context;

        public TitlesController(NominaDbContext context)
        {
            _context = context;
        }

        // GET: Titles
        public async Task<IActionResult> Index()
        {
            var nominaDbContext = _context.Titles.Include(t => t.EmpNoNavigation);
            return View(await nominaDbContext.ToListAsync());
        }

        // GET: Titles/Details?empNo=1&title1=Jefe%20RRHH&fromDate=2023-01-15
        public async Task<IActionResult> Details(int? empNo, string? title1, string? fromDate)
        {
            if (empNo == null || string.IsNullOrWhiteSpace(title1) || string.IsNullOrWhiteSpace(fromDate))
                return NotFound();

            var title = await _context.Titles
                .Include(t => t.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.Title1 == title1 &&
                    m.FromDate == fromDate);

            if (title == null) return NotFound();

            return View(title);
        }

        // GET: Titles/Create
        public IActionResult Create()
        {
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo");
            return View();
        }

        // POST: Titles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpNo,Title1,FromDate,ToDate")] Title title)
        {
            if (ModelState.IsValid)
            {
                _context.Add(title);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", title.EmpNo);
            return View(title);
        }

        // GET: Titles/Edit?empNo=1&title1=Jefe%20RRHH&fromDate=2023-01-15
        public async Task<IActionResult> Edit(int? empNo, string? title1, string? fromDate)
        {
            if (empNo == null || string.IsNullOrWhiteSpace(title1) || string.IsNullOrWhiteSpace(fromDate))
                return NotFound();

            var title = await _context.Titles.FindAsync(empNo, title1, fromDate);
            if (title == null) return NotFound();

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", title.EmpNo);
            return View(title);
        }

        // POST: Titles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int empNo, string title1, string fromDate,
            [Bind("EmpNo,Title1,FromDate,ToDate")] Title title)
        {
            if (empNo != title.EmpNo || title1 != title.Title1 || fromDate != title.FromDate)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(title);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool exists = await _context.Titles.AnyAsync(e =>
                        e.EmpNo == empNo && e.Title1 == title1 && e.FromDate == fromDate);

                    if (!exists) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", title.EmpNo);
            return View(title);
        }

        // GET: Titles/Delete?empNo=1&title1=Jefe%20RRHH&fromDate=2023-01-15
        public async Task<IActionResult> Delete(int? empNo, string? title1, string? fromDate)
        {
            if (empNo == null || string.IsNullOrWhiteSpace(title1) || string.IsNullOrWhiteSpace(fromDate))
                return NotFound();

            var title = await _context.Titles
                .Include(t => t.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.Title1 == title1 &&
                    m.FromDate == fromDate);

            if (title == null) return NotFound();

            return View(title);
        }

        // POST: Titles/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int empNo, string title1, string fromDate)
        {
            var title = await _context.Titles.FindAsync(empNo, title1, fromDate);
            if (title != null)
            {
                _context.Titles.Remove(title);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}