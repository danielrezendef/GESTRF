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
        private Contexto _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UsuarioController(Contexto context)
        {
            _context = context;
        }

        // GET: Estudantes
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var usuario = from usu in _context.Usuario
                              select usu;

                return View(usuario);
            }
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Usuario
                    .SingleOrDefaultAsync(m => m.UsuarioId == id);

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
            else
            {
                return RedirectToAction("Index", "Login");
            }
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
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

                string? img = string.Empty;
                if (usuario.Image != null)
                {
                    img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(usuario.Image));
                }
                else
                    img = string.Format(@"../../dist/img/user.jpg");

                ViewBag.imageDataUrl = img;

                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, Usuario usuario)
        {
            if (id == null)
            {
                return NotFound();
            }
            Usuario UsuarioBd = _context.Usuario.AsNoTracking().FirstOrDefault(s => s.UsuarioId == id);
            if (UsuarioBd == null)
            {
                return NotFound();
            }

            usuario.UsuarioId = id.Value;
            if (usuario.Foto != null)
            {
                var arqImage = new BinaryReader(usuario.Foto.OpenReadStream());
                usuario.Image = arqImage.ReadBytes((int)usuario.Foto.Length);
                var base64img = Convert.ToBase64String(usuario.Image);
            }
            else
            {
                usuario.Image = UsuarioBd.Image;
            }
            try
            {
                _context.Usuario.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException /* ex */)
            {
                //Logar o erro(descomente a variável ex e escreva um log
                ModelState.AddModelError("", "Não foi possível salvar. " +
                    "Tente novamente, e se o problema persistir " +
                    "chame o suporte.");
            }
            string? img = string.Empty;
            if (UsuarioBd.Image != null)
            {
                img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(UsuarioBd.Image));
            }
            else
                img = string.Format(@"../../dist/img/user.jpg");

            ViewBag.imageDataUrl = img;
            return View(UsuarioBd);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Remove(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
