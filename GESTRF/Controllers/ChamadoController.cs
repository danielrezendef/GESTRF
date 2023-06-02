using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GESTRF.Models;
using GESTRF;
using Microsoft.AspNetCore.Authorization;

namespace GESTRF.Controllers
{
    public class ChamadoController : Controller
    {
        private readonly Contexto _context;

        public ChamadoController(Contexto context)
        {
            _context = context;
        }

        // GET: Estudantes
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                var chamado = from Chamado in _context.Chamado
                              select Chamado;

                return View(chamado);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
