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

#if ANDROID
        connectionString = "Server=10.0.2.2,1433;Database=GestionAcademica;User Id=pae_movil;Password=Movil2026*;TrustServerCertificate=True;";
#else
        connectionString = "Server=localhost,1433;Database=GestionAcademica;User Id=pae_movil;Password=Movil2026*;TrustServerCertificate=True;";
#endif

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddScoped<AuthService>();

        return builder.Build();
    }
}