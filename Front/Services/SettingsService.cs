using ParckingAuto.DTO;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class SettingsService
{
    private readonly HttpClient _http;

    public SettingsService(HttpClient http)
    {
        _http = http;
    }

    public async Task SaveSettingsAsync(SettingsDto settings)
        => await _http.PutAsJsonAsync("Settings/update", settings);
}
