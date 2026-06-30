using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParckingAuto.DTO;

public class VehiculeService
{
    private readonly HttpClient _http;

    public VehiculeService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<VehiculeDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<VehiculeDto>>("Vehicules");

    public async Task<VehiculeDto> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<VehiculeDto>($"Vehicules/{id}");

    public async Task AddAsync(VehiculeDto vehicule)
        => await _http.PostAsJsonAsync("Vehicules", vehicule);

    public async Task UpdateAsync(int id, VehiculeDto vehicule)
        => await _http.PutAsJsonAsync($"Vehicules/{id}", vehicule);

    public async Task DeleteAsync(int id)
        => await _http.DeleteAsync($"Vehicules/{id}");
}
