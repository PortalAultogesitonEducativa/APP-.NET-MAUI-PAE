using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;
using PAE_APP_MOVIL_1._0.Models;

namespace PAE_APP_MOVIL_1._0.Services
{
    public class NotasService
    {
        private readonly AppDbContext _context;

        public NotasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Calificacion>> ObtenerNotasAsync(int idUsuarioEstudiante)
        {
            return await _context.Calificaciones
                .Where(c => c.ID_Estudiante == idUsuarioEstudiante)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<decimal?> ObtenerPromedioAsync(int idUsuarioEstudiante)
        {
            var notas = await _context.Calificaciones
                .Where(c => c.ID_Estudiante == idUsuarioEstudiante)
                .Select(c => c.Nota)
                .ToListAsync();

            if (notas.Count == 0)
                return null;

            return Math.Round(notas.Average(), 2);
        }
    }
}