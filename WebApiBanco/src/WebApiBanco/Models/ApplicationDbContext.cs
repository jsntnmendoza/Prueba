using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<OPago> OPagos { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Estado> Estados { get; set; }
    }
}
