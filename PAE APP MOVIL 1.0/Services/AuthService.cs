using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;

namespace PAE_APP_MOVIL_1._0.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResultado> IniciarSesionAsync(string usuarioOCorreo, string password)
        {
            string? nombreUsuarioReal = await _context.Usuarios
                .Where(u => u.NOMBRE_USUARIO == usuarioOCorreo || u.CORREO_ELECTRONICO == usuarioOCorreo)
                .Select(u => u.NOMBRE_USUARIO)
                .FirstOrDefaultAsync();

            if (nombreUsuarioReal == null)
            {
                return new LoginResultado
                {
                    Codigo = 2,
                    Mensaje = "Usuario o contraseña incorrectos."
                };
            }

            var resultado = await _context.LoginResultados
                .FromSqlInterpolated($"EXEC sp_LoginUsuario @NombreUsuario={nombreUsuarioReal}, @PasswordHash={password}")
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new LoginResultado
            {
                Codigo = 2,
                Mensaje = "No se pudo validar el usuario."
            };
        }

        public async Task<(RecuperacionResultado resultado, int? idUsuario)> SolicitarRecuperacionAsync(string usuarioOCorreo)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.NOMBRE_USUARIO == usuarioOCorreo || u.CORREO_ELECTRONICO == usuarioOCorreo)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return (new RecuperacionResultado
                {
                    Codigo = "Error",
                    Mensaje = "Usuario o correo no encontrado."
                }, null);
            }

            var resultado = await _context.RecuperacionResultados
                .FromSqlInterpolated($"EXEC sp_RecuperarPassword @IdUsuario={usuario.ID_Usuario}")
                .ToListAsync();

            var r = resultado.FirstOrDefault() ?? new RecuperacionResultado
            {
                Codigo = "Error",
                Mensaje = "No se pudo generar el token."
            };

            return (r, usuario.ID_Usuario);
        }

        public async Task<CambioPasswordResultado> CambiarPasswordAsync(int idUsuario, string nuevaPassword, string token)
        {
            var resultado = await _context.CambioPasswordResultados
                .FromSqlInterpolated($"EXEC sp_CambiarPassword @IdUsuario={idUsuario}, @NuevoHash={nuevaPassword}, @Token={token}")
                .ToListAsync();

            return resultado.FirstOrDefault() ?? new CambioPasswordResultado
            {
                Codigo = "Error",
                Mensaje = "No se pudo cambiar la contraseña."
            };
        }
    }
}