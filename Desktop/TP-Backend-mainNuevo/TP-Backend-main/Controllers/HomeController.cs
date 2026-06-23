using System.Diagnostics;
using GestorDespacho.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestorDespacho.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AcercaDe()
        {
            return View();
        }

    }
}
