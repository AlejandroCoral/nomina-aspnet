using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data;
using SistemaNominaMVC.Models;
using Microsoft.AspNetCore.Authorization; // Importante para la seguridad

namespace SistemaNominaMVC.Controllers
{
    // RF-12: Solo usuarios autenticados pueden ver la auditoría
    [Authorize]
    public class Log_AuditoriaSalariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Log_AuditoriaSalariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Log_AuditoriaSalarios
        // RF-10: Implementamos búsqueda por usuario o empleado
        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.CurrentFilter = searchString;
            var logs = from l in _context.Log_AuditoriaSalarios select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(s => s.usuario.Contains(searchString)
                                    || s.emp_no.ToString().Contains(searchString));
            }

            // Ordenamos por fecha para ver lo más reciente primero
            return View(await logs.OrderByDescending(l => l.fechaActualiz).ToListAsync());
        }

        // GET: Log_AuditoriaSalarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var log = await _context.Log_AuditoriaSalarios.FirstOrDefaultAsync(m => m.id == id);

            if (log == null) return NotFound();

            return View(log);
        }

        /* ATENCIÓN: Se eliminan los métodos Create, Edit y Delete. 
           La auditoría es de SOLO LECTURA para el usuario. 
           Se alimenta automáticamente desde el SalariesController.
        */
    }
}