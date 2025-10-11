﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VideoclubWebApp.Models;
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
        public DbSet<Rentas> Rentas { get; set; }
        public DbSet<TipoArticulo> TiposArticulos { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
    }
}