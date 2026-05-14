using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using Oficcios360.Models;


namespace Oficcios360.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Carta> Cartas { get; set; }
    }
}