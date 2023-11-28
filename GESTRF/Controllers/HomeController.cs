using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GESTRF.Models;
using Microsoft.EntityFrameworkCore;

namespace GESTRF.Controllers
{
    public class HomeController : Controller
    {
        private readonly Contexto _contexto;
        private readonly ILogger<HomeController> _logger;
        private readonly ILogger<UsuarioController> _Usuario;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Nosso Time";

            if (User.Identity.IsAuthenticated)
            {
                var usuario = from usu in _contexto.Usuario
                              select usu;

                return View(usuario);
            }
           
            return RedirectToAction("Index", "Login");
        }





        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Privacy()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
