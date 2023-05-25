using GESTRF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GESTRF
{
    public class DbInitializer
    {
        public static void Initialize(Contexto context)
        {
            context.Database.EnsureCreated();


            // Procura por qualquer Usuário
            if (context.Usuario.Any())
                return;  //O banco foi inicializado


            var usuario = new Usuario[]
           {
            new Usuario{Nome="Filipe Silveira" ,Username="filipe" ,Senha="123", Email="filipe@sigti.com.br",Perfil = "1",Image = "../../dist/img/avatar5.png"},
            new Usuario{Nome="Daniel Rezende" ,Username="daniel" ,Senha="345",Email="daniel@sigti.com.br",Perfil = "1",Image = "../../dist/img/avatar4.png"},
            new Usuario{Nome="Hebert Gonçaves" ,Username="hebert" ,Senha="567",Email="hebert@sigti.com.br",Perfil = "1", Image = "../../dist/img/avatar3.png"},
           };
            foreach (Usuario s in usuario)
            {
                context.Usuario.Add(s);
            }
            context.SaveChanges();
        }


    }
}
