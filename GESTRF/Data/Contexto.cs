using GESTRF.Models;
using Microsoft.EntityFrameworkCore;

namespace GESTRF
{
    public class Contexto : DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Chamado> Chamado { get; set; }
        public DbSet<Mensagem> Mensagem { get; set; }


        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }

        public Contexto()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>().ToTable("Usuario");
            builder.Entity<Chamado>().ToTable("Chamado");
            builder.Entity<Mensagem>().ToTable("Mensagem");


            builder.Entity<Chamado>(x =>
            {
                x.Ignore(y => y.Alteracao);
            });
            builder.Entity<Usuario>(x =>
            {
                x.Ignore(y => y.Foto);
            });
            //builder.Entity<Usuario>(x =>
            //{
            //    x.Property(y => y.Nome).HasMaxLength(50).IsRequired();
            //    x.Property(y => y.Username).HasMaxLength(50).IsRequired();
            //    x.Property(y => y.Senha).HasMaxLength(50).IsRequired();
            //});
        }
    }
}
