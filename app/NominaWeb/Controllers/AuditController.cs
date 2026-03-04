using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NominaWeb.Data;

namespace NominaWeb.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly NominaDbContext _context;

        public AuditController(NominaDbContext context)
        {
            _context = context;
        }

        // GET: /Audit
        public async Task<IActionResult> Index()
        {
            var logs = await _context.LogAuditoriaSalarios
                .Include(l => l.EmpNoNavigation)
                .OrderByDescending(l => l.FechaActualiz) // si FechaActualiz es DateOnly funciona igual
                .ThenByDescending(l => l.Id)
                .ToListAsync();

            return View(logs);
        }
    }
}
