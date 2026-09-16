using Microsoft.Extensions.DependencyInjection;
using PAE_APP_MOVIL_1._0.Services;

namespace PAE_APP_MOVIL_1._0
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly AuthService _authService;

        public ForgotPasswordPage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void OnEnviarClicked(object sender, EventArgs e)
        {
            string usuarioOCorreo = UsuarioEntry.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(usuarioOCorreo))
            {
                await DisplayAlert("Campo vacío", "Ingresa tu usuario o correo.", "Aceptar");
                return;
            }

            EnviarBtn.IsEnabled = false;
            CargandoIndicator.IsVisible = true;
            CargandoIndicator.IsRunning = true;

            try
            {
                var (resultado, idUsuario) = await _authService.SolicitarRecuperacionAsync(usuarioOCorreo);

                if (resultado.Codigo != "OK" || idUsuario == null || string.IsNullOrEmpty(resultado.Token))
                {
                    await DisplayAlert("Error", resultado.Mensaje, "Aceptar");
                    return;
                }

                // SOLO PARA PRUEBAS: en producción el token nunca debe mostrarse en la app,
                // solo debe llegar por correo (tu proyecto web ya lo hace con MailKit).
                await DisplayAlert("Token generado (solo pruebas)", resultado.Token, "Continuar");

                var resetPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<ResetPasswordPage>();
                resetPage.ConfigurarDatos(idUsuario.Value, resultado.Token);
                await Navigation.PushAsync(resetPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de conexión", ex.Message, "Aceptar");
            }
            finally
            {
                EnviarBtn.IsEnabled = true;
                CargandoIndicator.IsVisible = false;
                CargandoIndicator.IsRunning = false;
            }
        }
    }
}