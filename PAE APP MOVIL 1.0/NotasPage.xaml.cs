using PAE_APP_MOVIL_1._0.Services;

namespace PAE_APP_MOVIL_1._0
{
    public partial class NotasPage : ContentPage
    {
        private readonly NotasService _notasService;
        private int _idEstudiante;

        public NotasPage(NotasService notasService)
        {
            InitializeComponent();
            _notasService = notasService;
        }

        public void ConfigurarEstudiante(int idUsuarioEstudiante, string nombreEstudiante)
        {
            _idEstudiante = idUsuarioEstudiante;
            TituloLabel.Text = $"Notas de {nombreEstudiante}";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarNotasAsync();
        }

        private async Task CargarNotasAsync()
        {
            CargandoIndicator.IsVisible = true;
            CargandoIndicator.IsRunning = true;

            try
            {
                var notas = await _notasService.ObtenerNotasAsync(_idEstudiante);
                var promedio = await _notasService.ObtenerPromedioAsync(_idEstudiante);

                PromedioLabel.Text = promedio.HasValue
                    ? $"Promedio actual: {promedio.Value}"
                    : "Sin notas registradas";

                NotasCollection.ItemsSource = notas;
                SinNotasLabel.IsVisible = notas.Count == 0;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                CargandoIndicator.IsVisible = false;
                CargandoIndicator.IsRunning = false;
            }
        }
    }
}