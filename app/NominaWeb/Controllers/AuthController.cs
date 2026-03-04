using Microsoft.AspNetCore.Mvc;
using NominaWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace NominaWeb.Controllers
{
    [AllowAnonymous] // ✅ Este controlador es público (Login/Logout)
    public class AuthController : Controller
    {
        private readonly NominaDbContext _context;

        public AuthController(NominaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error = "Ingrese usuario y clave.";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Usuario == usuario && u.Clave == clave);

            if (user == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Usuario),
                new Claim(ClaimTypes.NameIdentifier, user.EmpNo.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // ✅ Al loguearse, manda a Employees (puedes cambiar a Home si quieres)
            return RedirectToAction("Index", "Employees");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}