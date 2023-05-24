using GESTRF.Models;
using Microsoft.EntityFrameworkCore;

namespace GESTRF
{
    public class Contexto : DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }

        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>().ToTable("Usuario");

            //builder.Entity<Usuario>(x =>
            //{
            //    x.Property(y => y.Nome).HasMaxLength(50).IsRequired();
            //    x.Property(y => y.Username).HasMaxLength(50).IsRequired();
            //    x.Property(y => y.Senha).HasMaxLength(50).IsRequired();
            //});
        }
    }
}
