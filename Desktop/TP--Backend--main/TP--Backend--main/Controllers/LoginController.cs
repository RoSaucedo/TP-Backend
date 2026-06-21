using Microsoft.AspNetCore.Mvc;
using GestorDespacho.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDespacho.Controllers
{
    public class LoginController : Controller
    {
        private readonly AplicationDbContext _context;

        public LoginController(AplicationDbContext context)
        {
            _context = context;
        }

        // Muestra el formulario 
        public IActionResult Index()
        {
            return View();
        }

        // Procesa el formulario cuando el usuario hace clic en Ingresar
        [HttpPost]
        public async Task<IActionResult> Index(string nombreUsuario, string password)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Password == password);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            // Guardamos el usuario
            HttpContext.Session.SetString("UsuarioNombre", usuario.NombreUsuario);
            HttpContext.Session.SetString("UsuarioRol", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }

        // Cierra la sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
