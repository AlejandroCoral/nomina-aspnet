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
    public class DeptEmpsController : Controller
    {
        private readonly NominaDbContext _context;

        public DeptEmpsController(NominaDbContext context)
        {
            _context = context;
        }

        // GET: DeptEmps
        public async Task<IActionResult> Index()
        {
            var nominaDbContext = _context.DeptEmps
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation);

            return View(await nominaDbContext.ToListAsync());
        }

        // ✅ DETAILS con clave compuesta
        public async Task<IActionResult> Details([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptEmp = await _context.DeptEmps
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptEmp == null) return NotFound();

            return View(deptEmp);
        }

        // GET: DeptEmps/Create
        public IActionResult Create()
        {
            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo");
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo");
            return View();
        }

        // POST: DeptEmps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpNo,DeptNo,FromDate,ToDate")] DeptEmp deptEmp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deptEmp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptEmp.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptEmp.EmpNo);
            return View(deptEmp);
        }

        // ✅ EDIT GET con clave compuesta
        public async Task<IActionResult> Edit([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptEmp = await _context.DeptEmps
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptEmp == null) return NotFound();

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptEmp.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptEmp.EmpNo);
            return View(deptEmp);
        }

        // ✅ EDIT POST con clave compuesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate,
            [Bind("EmpNo,DeptNo,FromDate,ToDate")] DeptEmp deptEmp)
        {
            if (empNo != deptEmp.EmpNo || deptNo != deptEmp.DeptNo || fromDate != deptEmp.FromDate)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deptEmp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeptEmpExists(deptEmp.EmpNo, deptEmp.DeptNo, deptEmp.FromDate))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptNo"] = new SelectList(_context.Departments, "DeptNo", "DeptNo", deptEmp.DeptNo);
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", deptEmp.EmpNo);
            return View(deptEmp);
        }

        // ✅ DELETE GET con clave compuesta
        public async Task<IActionResult> Delete([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptEmp = await _context.DeptEmps
                .Include(d => d.DeptNoNavigation)
                .Include(d => d.EmpNoNavigation)
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptEmp == null) return NotFound();

            return View(deptEmp);
        }

        // ✅ DELETE POST con clave compuesta
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromQuery] int empNo, [FromQuery] int deptNo, [FromQuery] string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var deptEmp = await _context.DeptEmps
                .FirstOrDefaultAsync(m =>
                    m.EmpNo == empNo &&
                    m.DeptNo == deptNo &&
                    m.FromDate == fromDate);

            if (deptEmp != null)
            {
                _context.DeptEmps.Remove(deptEmp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DeptEmpExists(int empNo, int deptNo, string fromDate)
        {
            return _context.DeptEmps.Any(e =>
                e.EmpNo == empNo &&
                e.DeptNo == deptNo &&
                e.FromDate == fromDate);
        }
    }
}