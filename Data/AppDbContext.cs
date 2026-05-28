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
        public DbSet<RegistroActividad> RegistroActividades { get; set; }
        public DbSet<Informe> Informes { get; set; }

        // --- NUEVA TABLA PARA HISTORIAL ACADÉMICO ---
        public DbSet<FormacionAcademica> FormacionesAcademicas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzamos a que busquen exactamente en minúsculas y en el esquema público
            modelBuilder.Entity<RegistroActividad>().ToTable("registro_actividades", schema: "public");
            modelBuilder.Entity<Informe>().ToTable("informes", schema: "public");

            // --- MAPEADO FORMAL PARA POSTGRESQL (NUEVO) ---
            modelBuilder.Entity<FormacionAcademica>().ToTable("formaciones_academicas", schema: "public");
        }
    }
}