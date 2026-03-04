using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data;
using SistemaNominaMVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SistemaNominaMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string clave)
        {
            // Buscamos el usuario (EF ya no buscará la columna 'rol')
            var user = await _context.Users.FirstOrDefaultAsync(u => u.usuario == usuario);

            if (user != null && BCrypt.Net.BCrypt.Verify(clave, user.clave))
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.usuario),
                    new Claim(ClaimTypes.Role, "Admin") // Rol asignado en memoria
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string usuario, string clave, int emp_no)
        {
            var nuevoUsuario = new User
            {
                emp_no = emp_no,
                usuario = usuario,
                clave = BCrypt.Net.BCrypt.HashPassword(clave)
            };

            try
            {
                _context.Users.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: Verifique que el ID de empleado (" + emp_no + ") exista en la tabla employees.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index");
        }
    }
}