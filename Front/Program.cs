using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Front.Services;

namespace Front
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Base URL de ton API
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7275/api/") });

            // Authentification JWT
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UtilisateurService>();
            builder.Services.AddScoped<VehiculeService>();
            builder.Services.AddScoped<ChauffeurService>();
            builder.Services.AddScoped<MouvementService>();
            builder.Services.AddScoped<CarburantService>();
            builder.Services.AddScoped<MaintenanceService>();
            builder.Services.AddScoped<AlerteService>();
            builder.Services.AddScoped<AuditService>();
            builder.Services.AddScoped<StatistiqueService>();
            builder.Services.AddScoped<SettingsService>();
            builder.Services.AddScoped<OcrService>();
            builder.Services.AddSingleton<DialogService>();


            await builder.Build().RunAsync();
        }
    }
}
