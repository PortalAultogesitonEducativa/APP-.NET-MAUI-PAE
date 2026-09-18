using Microsoft.EntityFrameworkCore;
using PAE_APP_MOVIL_1._0.Data;
using PAE_APP_MOVIL_1._0.Services;
using PAE_APP_MOVIL_1._0;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        string connectionString;


        connectionString = "Server=10.0.0.146,1433;Database=GestionAcademica;User Id=pae_app;Password=pae2025sena;TrustServerCertificate=True;";


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<ResetPasswordPage>();
        builder.Services.AddScoped<NotasService>();
        builder.Services.AddTransient<StudentDashboardPage>();
        builder.Services.AddTransient<NotasPage>();
        builder.Services.AddScoped<AcudienteService>();
        builder.Services.AddScoped<CitacionesService>();
        builder.Services.AddTransient<AcudienteDashboardPage>();
        builder.Services.AddTransient<CitacionesPage>();
        builder.Services.AddScoped<AsistenciaService>();
        builder.Services.AddTransient<AsistenciaPage>();

        return builder.Build();
    }
}