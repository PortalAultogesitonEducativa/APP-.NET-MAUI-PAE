using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Models;

namespace PAE_APP_MOVIL_1._0.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Citacion> Citaciones { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<PadreTutor> PadresTutores { get; set; }
        public DbSet<EstudiantePadre> EstudiantesPadres { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<LoginResultado> LoginResultados { get; set; }
        public DbSet<RecuperacionResultado> RecuperacionResultados { get; set; }
        public DbSet<CambioPasswordResultado> CambioPasswordResultados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calificacion>().ToTable("Calificaciones");
            modelBuilder.Entity<Citacion>().ToTable("CITACION");
            modelBuilder.Entity<Estudiante>().ToTable("ESTUDIANTE");
            modelBuilder.Entity<PadreTutor>().ToTable("PADRE_TUTOR");
            modelBuilder.Entity<Asistencia>().ToTable("ASISTENCIA");

            modelBuilder.Entity<EstudiantePadre>()
                .HasKey(ep => new { ep.ID_PADRE, ep.ID_ESTUDIANTE });

            modelBuilder.Entity<LoginResultado>().HasNoKey().ToView(null);
            modelBuilder.Entity<RecuperacionResultado>().HasNoKey().ToView(null);
            modelBuilder.Entity<CambioPasswordResultado>().HasNoKey().ToView(null);
        }
    }

    public class LoginResultado
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
    }

    public class RecuperacionResultado
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string? Token { get; set; }
    }

    public class CambioPasswordResultado
    {
        public string Codigo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
    }
}