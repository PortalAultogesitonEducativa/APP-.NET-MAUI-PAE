using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;
using PAE_APP_MOVIL_1._0.Models;

namespace PAE_APP_MOVIL_1._0.Services
{
    public class AsistenciaService
    {
        private readonly AppDbContext _context;

        public AsistenciaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Asistencia>> ObtenerAsistenciaAsync(int idUsuarioEstudiante)
        {
            var idEstudiante = await ResolverIdEstudianteAsync(idUsuarioEstudiante);

            if (idEstudiante == null)
                return new List<Asistencia>();

            return await _context.Asistencias
                .Where(a => a.id_estudiante == idEstudiante)
                .OrderByDescending(a => a.fecha)
                .ToListAsync();
        }

        public async Task<int> ContarFallasAsync(int idUsuarioEstudiante)
        {
            var idEstudiante = await ResolverIdEstudianteAsync(idUsuarioEstudiante);

            if (idEstudiante == null)
                return 0;

            return await _context.Asistencias
                .Where(a => a.id_estudiante == idEstudiante && a.estado == "Ausente")
                .CountAsync();
        }

        private async Task<int?> ResolverIdEstudianteAsync(int idUsuarioEstudiante)
        {
            return await _context.Estudiantes
                .Where(e => e.id_usuario == idUsuarioEstudiante)
                .Select(e => (int?)e.id_estudiante)
                .FirstOrDefaultAsync();
        }
    }
}