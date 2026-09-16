using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;

namespace PAE_APP_MOVIL_1._0
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly AppDbContext _context;

        public MainPage(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void OnTestDbClicked(object sender, EventArgs e)
        {
            try
            {
                int total = await _context.Usuarios.CountAsync();
                ResultLabel.Text = $"Conexión exitosa. Usuarios encontrados: {total}";
                await DisplayAlert("Conexión OK", $"Se encontraron {total} usuarios en GestionAcademica.", "Aceptar");
            }
            catch (Exception ex)
            {
                ResultLabel.Text = "Error de conexión";
                await DisplayAlert("Error de conexión", ex.Message, "Aceptar");
            }
        }
    }
}