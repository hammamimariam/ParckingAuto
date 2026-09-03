using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParckingAuto.DTO;

public class AuditService
{
    private readonly HttpClient _http;

    public AuditService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AuditDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<AuditDto>>("Audit");

    public async Task<AuditDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<AuditDto>($"Audit/{id}");
}
