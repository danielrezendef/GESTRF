using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GESTRF.Models;
using GESTRF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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


        public async Task<IActionResult> Create([Bind("ChamadoId,Assunto,Tecnico,Setor,DataCri,Status,Doc")] Chamado chamado)
        {
            try
            {

                if(chamado != null)
                {
                    chamado.Status = "Aberto";
                    chamado.DataCri = DateTime.Now;
                    chamado.Tecnico = "Daniel";

                    if (string.IsNullOrEmpty(chamado.Assunto))
                    {

                    }
                    if (string.IsNullOrEmpty(chamado.Status))
                    {

                    }


                    _context.Add(chamado);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                

            }
            catch (DbUpdateException /* ex */)
            {
                //Logar o erro (descomente a variável ex e escreva um log
                ModelState.AddModelError("", "Não foi possível salvar. " +
                    "Tente novamente, e se o problema persistir " +
                    "chame o suporte.");
            }
            return View(chamado);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamado
                .SingleOrDefaultAsync(m => m.ChamadoId == id);

            //var estudante = await _context.Usuario
            //    .Include(s => s.Matriculas)
            //    .ThenInclude(e => e.Curso)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(m => m.EstudanteID == id);

            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }


    }
}
