using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;

namespace PAE_APP_MOVIL_1._0.Services
{
    public class HijoInfo
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class AcudienteService
    {
        private readonly AppDbContext _context;

        public AcudienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HijoInfo>> ObtenerHijosAsync(int idUsuarioAcudiente)
        {
            var padre = await _context.PadresTutores
                .FirstOrDefaultAsync(p => p.id_usuario == idUsuarioAcudiente);

            if (padre == null)
                return new List<HijoInfo>();

            var idsEstudiantes = await _context.EstudiantesPadres
                .Where(ep => ep.ID_PADRE == padre.id_padre)
                .Select(ep => ep.ID_ESTUDIANTE)
                .ToListAsync();

            if (idsEstudiantes.Count == 0)
                return new List<HijoInfo>();

            var hijos = await _context.Estudiantes
                .Where(e => idsEstudiantes.Contains(e.id_estudiante) && e.id_usuario != null)
                .Select(e => new HijoInfo
                {
                    IdUsuario = e.id_usuario!.Value,
                    NombreCompleto = e.nombre + " " + e.apellido
                })
                .ToListAsync();

            return hijos;
        }
    }
}