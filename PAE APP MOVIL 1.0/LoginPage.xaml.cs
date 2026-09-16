using PAE_APP_MOVIL_1._0.Data;
using PAE_APP_MOVIL_1._0.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PAE_APP_MOVIL_1._0
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;
        private bool _passwordVisible = false;

        public LoginPage(AuthService authService, AppDbContext context)
        {
            InitializeComponent();
            _authService = authService;
            _context = context;
        }

        private void OnToggleMostrarPassword(object sender, EventArgs e)
        {
            _passwordVisible = !_passwordVisible;
            PasswordEntry.IsPassword = !_passwordVisible;
            TogglePasswordBtn.Text = _passwordVisible ? "🙈" : "👁";
        }

        private async void OnOlvidoPasswordTapped(object sender, EventArgs e)
        {
            var forgotPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<ForgotPasswordPage>();
            await Navigation.PushAsync(forgotPage);
        }

        private async void OnIngresarClicked(object sender, EventArgs e)
        {
            string usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Campos incompletos", "Ingresa tu usuario y contraseña.", "Aceptar");
                return;
            }

            IngresarBtn.IsEnabled = false;
            CargandoIndicator.IsVisible = true;
            CargandoIndicator.IsRunning = true;

            try
            {
                var resultado = await _authService.IniciarSesionAsync(usuario, password);

                if (resultado.Codigo != 0 || resultado.IdUsuario == null)
                {
                    await DisplayAlert("Error", resultado.Mensaje, "Aceptar");
                    return;
                }

                var usuarioLogueado = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.ID_Usuario == resultado.IdUsuario);

                if (usuarioLogueado == null)
                {
                    await DisplayAlert("Error", "No se pudo cargar la información del usuario.", "Aceptar");
                    return;
                }

                string rol = usuarioLogueado.ROL?.ToLower() ?? string.Empty;

                if (rol != "estudiante" && rol != "acudiente")
                {
                    await DisplayAlert("Acceso no permitido",
                        "Esta aplicación es exclusiva para estudiantes y acudientes. Usa el portal web con tu rol actual.",
                        "Aceptar");
                    return;
                }

                // Por ahora navegamos a MainPage como pantalla temporal.
                // Aquí luego distinguiremos entre pantalla de Estudiante y de Acudiente.
                await DisplayAlert("Bienvenido", $"Hola {usuarioLogueado.NOMBRES}, rol: {rol}", "Continuar");
                Application.Current.MainPage = new NavigationPage(
                    Application.Current.Handler.MauiContext.Services.GetRequiredService<MainPage>());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de conexión", ex.Message, "Aceptar");
            }
            finally
            {
                IngresarBtn.IsEnabled = true;
                CargandoIndicator.IsVisible = false;
                CargandoIndicator.IsRunning = false;
            }
        }
    }
}