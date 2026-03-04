using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data;
using SistemaNominaMVC.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaNominaMVC.Controllers
{
    [Authorize]
    public class SalariesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Salaries.ToListAsync());
        }

        // --- DETALLES ---
        // Cambiamos 'int? id' por los dos componentes de la llave
        public async Task<IActionResult> Details(int? empNo, string fromDate)
        {
            if (empNo == null || fromDate == null) return NotFound();

            var salary = await _context.Salaries
                .FirstOrDefaultAsync(m => m.emp_no == empNo && m.from_date == fromDate);

            if (salary == null) return NotFound();

            return View(salary);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("emp_no,salary,from_date,to_date")] Salary model)
        {
            if (!string.IsNullOrEmpty(model.to_date))
            {
                DateTime inicio = DateTime.Parse(model.from_date);
                DateTime fin = DateTime.Parse(model.to_date);
                if (fin < inicio)
                {
                    ModelState.AddModelError("to_date", "La fecha de fin no puede ser anterior a la de inicio.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(model);

                var auditoria = new Log_AuditoriaSalarios
                {
                    emp_no = model.emp_no,
                    salario = model.salary,
                    fechaActualiz = DateTime.Now,
                    usuario = User.Identity.Name ?? "Admin",
                    DetalleCambio = $"Alta de nuevo salario desde {model.from_date}"
                };

                _context.Log_AuditoriaSalarios.Add(auditoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // --- EDITAR (GET) ---
        public async Task<IActionResult> Edit(int? empNo, string fromDate)
        {
            if (empNo == null || fromDate == null) return NotFound();

            // FindAsync requiere todos los campos de la llave en el orden definido en el DbContext
            var salary = await _context.Salaries
                .FirstOrDefaultAsync(m => m.emp_no == empNo && m.from_date == fromDate);

            if (salary == null) return NotFound();
            return View(salary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int empNo, string fromDate, [Bind("emp_no,salary,from_date,to_date")] Salary salary)
        {
            // Validamos contra la llave compuesta
            if (empNo != salary.emp_no || fromDate != salary.from_date) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.emp_no, salary.from_date)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(salary);
        }

        // --- ELIMINAR (GET) ---
        public async Task<IActionResult> Delete(int? empNo, string fromDate)
        {
            if (empNo == null || fromDate == null) return NotFound();

            var salary = await _context.Salaries
                .FirstOrDefaultAsync(m => m.emp_no == empNo && m.from_date == fromDate);

            if (salary == null) return NotFound();
            return View(salary);
        }

        // --- ELIMINAR (POST) ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int empNo, string fromDate)
        {
            var salary = await _context.Salaries
                .FirstOrDefaultAsync(m => m.emp_no == empNo && m.from_date == fromDate);

            if (salary != null)
            {
                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int empNo, string fromDate)
        {
            return _context.Salaries.Any(e => e.emp_no == empNo && e.from_date == fromDate);
        }
    }
}