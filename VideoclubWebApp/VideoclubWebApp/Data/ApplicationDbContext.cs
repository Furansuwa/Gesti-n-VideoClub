using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoclubWebApp.Models.Articulos;
using VideoclubWebApp.Models.Elenco;
using VideoClubWebApp.Models;

namespace VideoClubWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas del sistema
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Rentas> Rentas { get; set; }
        public DbSet<TipoArticulo> TiposArticulos { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Elenco> Elencos { get; set; }
        public DbSet<ElencoArticulo> ElencoArticulos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurar la clave compuesta de ElencoArticulo
            builder.Entity<ElencoArticulo>().HasKey(
                ea => new { ea.ArticuloId, ea.ElencoId }
            );

            // Configurar Rentas con NoRenta como clave primaria
            builder.Entity<Rentas>()
                .HasKey(r => r.NoRenta);

            builder.Entity<Rentas>()
                .Property(r => r.NoRenta)
                .ValueGeneratedOnAdd();
        }
    }
}