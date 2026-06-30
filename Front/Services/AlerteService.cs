using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParckingAuto.DTO;

public class AlerteService
{
    private readonly HttpClient _http;

    public AlerteService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AlerteDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<AlerteDto>>("Alertes");
}
