using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data;
using SistemaNominaMVC.Models;

namespace SistemaNominaMVC.Controllers
{
    public class Dept_ManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Dept_ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dept_Manager
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dept_Managers.ToListAsync());
        }

        // GET: Dept_Manager/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept_Manager = await _context.Dept_Managers
                .FirstOrDefaultAsync(m => m.emp_no == id);
            if (dept_Manager == null)
            {
                return NotFound();
            }

            return View(dept_Manager);
        }

        // GET: Dept_Manager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dept_Manager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("emp_no,dept_no,from_date,to_date")] Dept_Manager model)
        {
            // VALIDACIÓN RF-11: Coherencia de fechas
            if (!string.IsNullOrEmpty(model.to_date) && model.to_date != "9999-01-01")
            {
                if (DateTime.Parse(model.to_date) < DateTime.Parse(model.from_date))
                {
                    ModelState.AddModelError("to_date", "La fecha de término no puede ser anterior al inicio de la gestión.");
                }
            }

            if (ModelState.IsValid)
            {
                // REGLA DE NEGOCIO (RF-05): Un único gerente activo por departamento
                // Buscamos al gerente que esté actualmente activo en ESE departamento
                var gerenteActual = await _context.Dept_Managers
                    .FirstOrDefaultAsync(m => m.dept_no == model.dept_no && m.to_date == "9999-01-01");

                if (gerenteActual != null)
                {
                    // "Despedimos" al gerente anterior de su cargo de manager
                    gerenteActual.to_date = model.from_date;
                    _context.Update(gerenteActual);
                }

                if (string.IsNullOrEmpty(model.to_date))
                {
                    model.to_date = "9999-01-01";
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dept_Manager/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept_Manager = await _context.Dept_Managers.FindAsync(id);
            if (dept_Manager == null)
            {
                return NotFound();
            }
            return View(dept_Manager);
        }

        // POST: Dept_Manager/Edit/1
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("emp_no,dept_no,from_date,to_date")] Dept_Manager dept_Manager)
        {
            if (id != dept_Manager.emp_no)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dept_Manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Dept_ManagerExists(dept_Manager.emp_no))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dept_Manager);
        }

        // GET: Dept_Manager/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dept_Manager = await _context.Dept_Managers
                .FirstOrDefaultAsync(m => m.emp_no == id);
            if (dept_Manager == null)
            {
                return NotFound();
            }

            return View(dept_Manager);
        }

        // POST: Dept_Manager/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dept_Manager = await _context.Dept_Managers.FindAsync(id);
            if (dept_Manager != null)
            {
                _context.Dept_Managers.Remove(dept_Manager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Dept_ManagerExists(int id)
        {
            return _context.Dept_Managers.Any(e => e.emp_no == id);
        }
    }
}
