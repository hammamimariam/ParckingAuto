using System.Net.Http.Json;
using ParckingAuto.DTO;

public class ChauffeurService
{
    private readonly HttpClient _http;

    public ChauffeurService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ChauffeurDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<ChauffeurDto>>("Chauffeurs");

    public async Task AddAsync(ChauffeurDto chauffeur)
        => await _http.PostAsJsonAsync("Chauffeurs", chauffeur);

    public async Task UpdateAsync(int id, ChauffeurDto chauffeur)
        => await _http.PutAsJsonAsync($"Chauffeurs/{id}", chauffeur);

    public async Task DeleteAsync(int id)
        => await _http.DeleteAsync($"Chauffeurs/{id}");
}
