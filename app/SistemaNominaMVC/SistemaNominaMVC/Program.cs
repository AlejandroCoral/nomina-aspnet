using Microsoft.EntityFrameworkCore;
using SistemaNominaMVC.Data; // Asegúrate de que este namespace coincida con tu proyecto

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la Base de Datos (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configuración de Autenticación por Cookies (RF-01, RF-12)
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Login/Index"; // Redirige aquí si no está logueado
        config.AccessDeniedPath = "/Home/Index";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// 3. Agregar servicios de Controladores y Vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Configurar el pipeline de solicitudes HTTP (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- EL ORDEN DE ESTOS DOS ES CRÍTICO ---
app.UseAuthentication(); // Primero identifica quién es (RF-01)
app.UseAuthorization();  // Luego revisa qué puede hacer (RF-12)
// ----------------------------------------

// 5. Configuración de Internacionalización (RF-14 - Formato de fecha y moneda)
var supportedCultures = new[] { "es-ES", "es-EC", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// 6. Configuración de Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Cambiado a Login para que sea lo primero al entrar

app.Run();