using Microsoft.Extensions.DependencyInjection;
using PAE_APP_MOVIL_1._0.Models;
using PAE_APP_MOVIL_1._0.Services;

namespace PAE_APP_MOVIL_1._0
{
    public partial class AcudienteDashboardPage : ContentPage
    {
        private readonly AcudienteService _acudienteService;
        private readonly NotasService _notasService;
        private Usuario? _usuario;
        private List<HijoInfo> _hijos = new();

        public AcudienteDashboardPage(AcudienteService acudienteService, NotasService notasService)
        {
            InitializeComponent();
            _acudienteService = acudienteService;
            _notasService = notasService;
        }

        public void ConfigurarUsuario(Usuario usuario)
        {
            _usuario = usuario;
            BienvenidaLabel.Text = $"Bienvenido/a, {usuario.NOMBRES} {usuario.APELLIDOS}";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_usuario == null)
                return;

            _hijos = await _acudienteService.ObtenerHijosAsync(_usuario.ID_Usuario);

            if (_hijos.Count == 0)
            {
                await DisplayAlert("Sin estudiantes vinculados",
                    "No se encontraron estudiantes asociados a tu cuenta.", "Aceptar");
                return;
            }

            HijoPicker.ItemsSource = _hijos.Select(h => h.NombreCompleto).ToList();
            HijoPicker.SelectedIndex = 0;
        }

        private async void OnHijoSeleccionado(object sender, EventArgs e)
        {
            await CargarPromedioAsync();
        }

        private async Task CargarPromedioAsync()
        {
            if (HijoPicker.SelectedIndex < 0 || _hijos.Count == 0)
                return;

            var hijo = _hijos[HijoPicker.SelectedIndex];
            var promedio = await _notasService.ObtenerPromedioAsync(hijo.IdUsuario);

            PromedioLabel.Text = promedio.HasValue
                ? $"Promedio actual: {promedio.Value}"
                : "Sin notas registradas";
        }

        private async void OnVerCalificacionesClicked(object sender, EventArgs e)
        {
            if (HijoPicker.SelectedIndex < 0 || _hijos.Count == 0)
                return;

            var hijo = _hijos[HijoPicker.SelectedIndex];
            var notasPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<NotasPage>();
            notasPage.ConfigurarEstudiante(hijo.IdUsuario, hijo.NombreCompleto);
            await Navigation.PushAsync(notasPage);
        }

        private async void OnAsistenciaClicked(object sender, EventArgs e)
        {
            if (HijoPicker.SelectedIndex < 0 || _hijos.Count == 0)
                return;

            var hijo = _hijos[HijoPicker.SelectedIndex];
            var asistenciaPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<AsistenciaPage>();
            asistenciaPage.ConfigurarEstudiante(hijo.IdUsuario, hijo.NombreCompleto);
            await Navigation.PushAsync(asistenciaPage);
        }

        private async void OnCitacionesClicked(object sender, EventArgs e)
        {
            if (HijoPicker.SelectedIndex < 0 || _hijos.Count == 0)
                return;

            var hijo = _hijos[HijoPicker.SelectedIndex];
            var citacionesPage = Application.Current.Handler.MauiContext.Services.GetRequiredService<CitacionesPage>();
            citacionesPage.ConfigurarEstudiante(hijo.IdUsuario, hijo.NombreCompleto);
            await Navigation.PushAsync(citacionesPage);
        }
    }
}