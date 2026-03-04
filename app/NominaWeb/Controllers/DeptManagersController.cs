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
    public class DeptManagersController : Controller
    {
        private readonly NominaDbContext _context;

        public DeptManagersController(NominaDbContext context)
        {
            _context = context;
        }

        // GET: DeptManagers
        public async Task<IActionResult> Index()
        {
            var nominaDbContext = _context.DeptManagers
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation);

            return View(await nominaDbContext.ToListAsync());
        }

        // ✅ DETAILS con clave compuesta
        public async Task<IActionResult> Details([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptManager = await _context.DeptManagers
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptManager == null) return NotFound();

            return View(deptManager);
        }

        // GET: DeptManagers/Create
        public IActionResult Create()
        {
            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo");
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo");
            return View();
        }

        // POST: DeptManagers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpNo,DeptNo,FromDate,ToDate")] DeptManager deptManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deptManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptManager.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptManager.EmpNo);
            return View(deptManager);
        }

        // ✅ EDIT GET con clave compuesta
        public async Task<IActionResult> Edit([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptManager = await _context.DeptManagers
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptManager == null) return NotFound();

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptManager.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptManager.EmpNo);
            return View(deptManager);
        }

        // ✅ EDIT POST con clave compuesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate,
            [Bind("EmpNo,DeptNo,FromDate,ToDate")] DeptManager deptManager)
        {
            if (empNo != deptManager.EmpNo || deptNo != deptManager.DeptNo || fromDate != deptManager.FromDate)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deptManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeptManagerExists(deptManager.EmpNo, deptManager.DeptNo, deptManager.FromDate))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptManager.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptManager.EmpNo);
            return View(deptManager);
        }

        // ✅ DELETE GET con clave compuesta
        public async Task<IActionResult> Delete([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptManager = await _context.DeptManagers
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptManager == null) return NotFound();

            return View(deptManager);
        }

        // ✅ DELETE POST con clave compuesta
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptManager = await _context.DeptManagers
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptManager != null)
            {
                _context.DeptManagers.Remove(deptManager);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DeptManagerExists(int empNo, int deptNo, string fromDate)
        {
            return _context.DeptManagers.Any(e =>
                e.EmpNo == empNo &&
                e.DeptNo == deptNo &&
                e.FromDate == fromDate);
        }
    }
}