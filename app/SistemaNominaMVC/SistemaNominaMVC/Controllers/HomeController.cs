using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data;
using SistemaNominaMVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace SistemaNominaMVC.Controllers
{
    // Solo usuarios autenticados pueden ver el Dashboard
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Conteo de Empleados y Departamentos
            ViewBag.TotalEmpleados = await _context.Employees.CountAsync();
            ViewBag.TotalDeptos = await _context.Departments.CountAsync();

            // 2. Cálculo de Presupuesto Mensual (RF-07)
            // Sumamos salarios de registros activos (vigencia 9999-01-01)
            var sumaSalarios = await _context.Salaries
                .Where(s => s.to_date == "9999-01-01" || s.to_date == null)
                .SumAsync(s => (double?)s.salary) ?? 0;

            ViewBag.PresupuestoMensual = sumaSalarios;

            // 3. Gerentes Activos (RF-05)
            ViewBag.TotalManagers = await _context.Dept_Managers
                .Where(m => m.to_date == "9999-01-01" || m.to_date == null)
                .CountAsync();

            // 4. Obtener los últimos 5 cambios de salario para el feed de actividad
            var ultimosCambios = await _context.Log_AuditoriaSalarios
                .OrderByDescending(l => l.fechaActualiz)
                .Take(5)
                .ToListAsync();

            return View(ultimosCambios);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}