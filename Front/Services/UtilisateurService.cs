using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ParckingAuto.DTO;

public class UtilisateurService
{
    private readonly HttpClient _http;
    public UtilisateurService(HttpClient http) => _http = http;

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<UserDto>>("Utilisateurs") ?? new List<UserDto>();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("Utilisateurs/register", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        var response = await _http.PutAsJsonAsync($"Utilisateurs/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"Utilisateurs/{id}");
        return response.IsSuccessStatusCode;
    }
}
