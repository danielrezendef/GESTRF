using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GESTRF.Models;
using GESTRF;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Routing.Template;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace GESTRF.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Contexto _context;
        private readonly IWebHostEnvironment webHostEnvironment;
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
                return NotFound();


            string? img = string.Empty;
            if (user.Image != null)
            {
                img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(user.Image));
            }
            else
                img = string.Format(@"../../dist/img/user.jpg");

            ViewBag.imageDataUrl = img;

            return View(user);
        }
        //[Bind("UsuarioId,Nome,Username,Senha,Email,Perfil,Image,Foto")]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var arqImage = new BinaryReader(usuario.Foto.OpenReadStream());
                        usuario.Image = arqImage.ReadBytes((int)usuario.Foto.Length);
                        var base64img = Convert.ToBase64String(usuario.Image);

                        _context.Add(usuario);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
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
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuario = await _context.Usuario.SingleOrDefaultAsync(m => m.UsuarioId == id);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    string? img = string.Empty;
        //    if (usuario.Image != null)
        //    {
        //        img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(usuario.Image));
        //    }
        //    else
        //        img = string.Format(@"../../dist/img/user.jpg");

        //    ViewBag.imageDataUrl = img;


        //    return View(usuario);
        //}

        //[HttpPost, ActionName("Edit")]
        //public async Task<IActionResult> EditPost(Usuario usu, int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var atualizarUsuario = await _context.Usuario.SingleOrDefaultAsync(s => s.UsuarioId == id);
        //    var arqImage = new BinaryReader(usu.Foto.OpenReadStream());
        //    atualizarUsuario.Image = arqImage.ReadBytes((int)usu.Foto.Length);
        //    if (await TryUpdateModelAsync<Usuario>(
        //        atualizarUsuario,
        //        "",
        //        s => s.Nome, s => s.Username, s => s.Perfil, s => s.Senha, s => s.Email, s => s.Image))
        //    {
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //            return View(atualizarUsuario);
        //        }
        //        catch (DbUpdateException /* ex */)
        //        {
        //            //Logar o erro (descomente a variável ex e escreva um log
        //            ModelState.AddModelError("", "Não foi possível salvar. " +
        //                "Tente novamente, e se o problema persistir " +
        //                "chame o suporte.");
        //        }
        //    }
        //    string? img = string.Empty;
        //    if (atualizarUsuario.Image != null)
        //    {
        //        img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(atualizarUsuario.Image));
        //    }
        //    else
        //        img = string.Format(@"../../dist/img/user.jpg");

        //    ViewBag.imageDataUrl = img;
        //    return View(atualizarUsuario);
        //}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.SingleOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Usuario _user)
        {
            if (id == null)
            {
                return NotFound();
            }
            var atualizarUsuario = await _context.Usuario.SingleOrDefaultAsync(s => s.UsuarioId == id);
            if (_user.Foto != null)
            {
                var arqImage = new BinaryReader(_user.Foto.OpenReadStream());
                _user.Image = arqImage.ReadBytes((int)_user.Foto.Length);
            }

            atualizarUsuario = _user;
            if (await TryUpdateModelAsync<Usuario>(
                atualizarUsuario,
                "",
                s => s.Nome, s => s.Username, s => s.Perfil, s => s.Senha, s => s.Email, s => s.Image))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return View(atualizarUsuario);
                }
                catch (DbUpdateException /* ex */)
                {
                    //Logar o erro (descomente a variável ex e escreva um log
                    ModelState.AddModelError("", "Não foi possível salvar. " +
                        "Tente novamente, e se o problema persistir " +
                        "chame o suporte.");
                }
            }
            return View(atualizarUsuario);
        }
    }
}
