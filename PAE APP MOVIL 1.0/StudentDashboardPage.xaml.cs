using Microsoft.Extensions.DependencyInjection;
using PAE_APP_MOVIL_1._0.Models;

namespace PAE_APP_MOVIL_1._0
{
    public partial class StudentDashboardPage : ContentPage
    {
        private Usuario? _usuario;

        public StudentDashboardPage()
        {
            InitializeComponent();
        }

        public void ConfigurarUsuario(Usuario usuario)
        {
            _usuario = usuario;
            BienvenidaLabel.Text = $"Bienvenido/a, {usuario.NOMBRES} {usuario.APELLIDOS}";
        }

        private async void OnVerNotasClicked(object sender, EventArgs e)
        {
            if (_usuario == null)
                return;

            var notasPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<NotasPage>();
            notasPage.ConfigurarEstudiante(_usuario.ID_Usuario, $"{_usuario.NOMBRES} {_usuario.APELLIDOS}");
            await Navigation.PushAsync(notasPage);
        }

        private async void OnHorarioClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Próximamente", "La consulta de horario estará disponible pronto.", "Aceptar");
        }

        private async void OnActividadesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Próximamente", "La consulta de actividades estará disponible pronto.", "Aceptar");
        }
    }
}