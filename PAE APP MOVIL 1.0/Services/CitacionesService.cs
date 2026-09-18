using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;
using PAE_APP_MOVIL_1._0.Models;

namespace PAE_APP_MOVIL_1._0.Services
{
    public class CitacionesService
    {
        private readonly AppDbContext _context;

        public CitacionesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Citacion>> ObtenerCitacionesAsync(int idUsuarioEstudiante)
        {
            var idEstudiante = await ResolverIdEstudianteAsync(idUsuarioEstudiante);

            if (idEstudiante == null)
                return new List<Citacion>();

            return await _context.Citaciones
                .Where(c => c.id_estudiante == idEstudiante)
                .OrderByDescending(c => c.fecha)
                .ToListAsync();
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