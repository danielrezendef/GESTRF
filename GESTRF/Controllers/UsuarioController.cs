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
    public class UsuarioController : Controller
    {
        private readonly Contexto _context;

        public UsuarioController(Contexto context)
        {
            _context = context;
        }

        // GET: Estudantes
        public async Task<IActionResult> Index(
            string ordem,
            string filtroAtual,
            string filtro,
            int? pagina)
        {
            ViewData["ordemAtual"] = ordem;
            ViewData["NomeParm"] = String.IsNullOrEmpty(ordem) ? "nome_desc" : "";
            ViewData["UsuarioIDParm"] = ordem == "UsuarioID" ? "UsuarioID_desc" : "UsuarioID";

            if (filtro != null)
            {
                pagina = 1;
            }
            else
            {
                filtro = filtroAtual;
            }

            ViewData["filtroAtual"] = filtro;

            var usuario = from usu in _context.Usuario
                          select usu;

            if (!String.IsNullOrEmpty(filtro))
            {
                usuario = usuario.Where(usu => usu.Nome.Contains(filtro)
                                       || usu.Email.Contains(filtro));
            }

            switch (ordem)
            {
                case "nome_desc":
                    usuario = usuario.OrderByDescending(est => est.Nome);
                    break;
                case "UsuarioID_desc":
                    usuario = usuario.OrderByDescending(est => est.UsuarioId);
                    break;
                default:
                    usuario = usuario.OrderBy(est => est.UsuarioId);
                    break;
            }

            if (User.Identity.IsAuthenticated)
            {
                return View(usuario);
            }
            return RedirectToAction("Index", "Login");
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Usuario
                .SingleOrDefaultAsync(m => m.UsuarioId == id);

            //var estudante = await _context.Usuario
            //    .Include(s => s.Matriculas)
            //    .ThenInclude(e => e.Curso)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(m => m.EstudanteID == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Create([Bind("UsuarioId,Nome,Username,Senha,Email,Perfil,Image")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
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
            return View(usuario);
        }



    }
}
