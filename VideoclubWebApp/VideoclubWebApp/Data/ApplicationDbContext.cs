using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VideoClubWebApp.Models;

namespace VideoClubWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tablas del sistema
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Renta> Rentas { get; set; }
    }
}
