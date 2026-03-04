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
    public class SalariesController : Controller
    {
        private readonly NominaDbContext _context;

        public SalariesController(NominaDbContext context)
        {
            _context = context;
        }

        private string GetAuditUser() => User?.Identity?.Name ?? "system";

        private void AddSalaryAudit(string detalle, Salary salary)
        {
            var log = new LogAuditoriaSalario
            {
                Usuario = GetAuditUser(),
                // En tu BD la fecha del log parece ser tipo DATE/DATETIME.
                // Si tu propiedad es DateOnly, deja así. Si da error, cambia por DateTime.Now.
                FechaActualiz = DateOnly.FromDateTime(DateTime.Now),
                DetalleCambio = detalle,
                Salario = salary.Salary1,
                EmpNo = salary.EmpNo
            };

            _context.LogAuditoriaSalarios.Add(log);
        }

        // GET: Salaries
        public async Task<IActionResult> Index()
        {
            var nominaDbContext = _context.Salaries.Include(s => s.EmpNoNavigation);
            return View(await nominaDbContext.ToListAsync());
        }

        // ✅ DETAILS: clave compuesta (EmpNo + FromDate) pero FromDate es string
        // /Salaries/Details?empNo=10001&fromDate=2023-01-01
        public async Task<IActionResult> Details(int empNo, string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var salary = await _context.Salaries
                .Include(s => s.EmpNoNavigation)
                .FirstOrDefaultAsync(s => s.EmpNo == empNo && s.FromDate == fromDate);

            if (salary == null) return NotFound();

            return View(salary);
        }

        // GET: Salaries/Create
        public IActionResult Create()
        {
            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo");
            return View();
        }

        // POST: Salaries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpNo,Salary1,FromDate,ToDate")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);

                AddSalaryAudit("INSERT: Nuevo salario registrado", salary);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", salary.EmpNo);
            return View(salary);
        }

        // ✅ EDIT GET: (EmpNo + FromDate string)
        public async Task<IActionResult> Edit(int empNo, string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var salary = await _context.Salaries
                .FirstOrDefaultAsync(s => s.EmpNo == empNo && s.FromDate == fromDate);

            if (salary == null) return NotFound();

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", salary.EmpNo);
            return View(salary);
        }

        // ✅ EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int empNo, string fromDate, [Bind("EmpNo,Salary1,FromDate,ToDate")] Salary salary)
        {
            if (empNo != salary.EmpNo || fromDate != salary.FromDate) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var oldSalary = await _context.Salaries
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.EmpNo == salary.EmpNo && s.FromDate == salary.FromDate);

                    _context.Update(salary);

                    if (oldSalary == null)
                        AddSalaryAudit("UPDATE: Salario actualizado", salary);
                    else
                        AddSalaryAudit($"UPDATE: Salario {oldSalary.Salary1} -> {salary.Salary1}", salary);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.EmpNo, salary.FromDate)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["EmpNo"] = new SelectList(_context.Employees, "EmpNo", "EmpNo", salary.EmpNo);
            return View(salary);
        }

        // ✅ DELETE GET
        public async Task<IActionResult> Delete(int empNo, string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var salary = await _context.Salaries
                .Include(s => s.EmpNoNavigation)
                .FirstOrDefaultAsync(s => s.EmpNo == empNo && s.FromDate == fromDate);

            if (salary == null) return NotFound();

            return View(salary);
        }

        // ✅ DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int empNo, string fromDate)
        {
            if (string.IsNullOrWhiteSpace(fromDate)) return NotFound();

            var salary = await _context.Salaries
                .FirstOrDefaultAsync(s => s.EmpNo == empNo && s.FromDate == fromDate);

            if (salary != null)
            {
                AddSalaryAudit("DELETE: Salario eliminado", salary);

                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int empNo, string fromDate)
        {
            return _context.Salaries.Any(e => e.EmpNo == empNo && e.FromDate == fromDate);
        }
    }
}
