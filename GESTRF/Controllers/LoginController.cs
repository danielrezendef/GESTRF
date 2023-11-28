using GESTRF.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace GESTRF.Controllers
{
    public class LoginController : Controller
    {
        private readonly Contexto _contexto;
        const string SessionNome = "Nome";
        const string SessionEmail = "Email";
        const string SessionImage = "Image";
        const string SessionPerfil = "Perfil";

        public LoginController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logar(string username, string senha, string lembrar)
        {
            Usuario usuario = _contexto.Usuario.AsNoTracking().FirstOrDefault(x => x.Username == username && x.Senha == senha);
            bool _lembrar = false;
            if (lembrar == "on")
                _lembrar = true;

            if (usuario != null)
            {
                int usuarioId = usuario.UsuarioId;
                string nome = usuario.Nome;
                string? base64img = string.Empty;
                if (usuario.Image != null)
                {
                    base64img = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(usuario.Image));
                }
                else
                    base64img = string.Format(@"../../dist/img/user.jpg");

               // < img src = "../../dist/img/user.jpg" class="user-image img-circle elevation-2" alt="User Image">

                List<Claim> direitosAcesso = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,usuarioId.ToString()),
                    new Claim(ClaimTypes.Name,nome),
                    new Claim(ClaimTypes.Role,usuario.Perfil)
                };

                var identity = new ClaimsIdentity(direitosAcesso, "Identity.Login");
                var userPrincipal = new ClaimsPrincipal(new[] { identity });

                await HttpContext.SignInAsync(userPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = _lembrar,
                        ExpiresUtc = DateTime.Now.AddHours(1)
                    });




                HttpContext.Session.SetString("SessionNome", usuario.Nome);
                HttpContext.Session.SetString("SessionEmail", usuario.Email);
                HttpContext.Session.SetString("SessionImage", base64img);
                HttpContext.Session.SetString("SessionPerfil", usuario.Perfil);


                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
