using ParckingAuto.DTO;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class DocumentService
{
    private readonly HttpClient _http;

    public DocumentService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DocumentDto>> GetAllAsync()
        => await _http.GetFromJsonAsync<List<DocumentDto>>("Documents");

    public async Task UploadAsync(IBrowserFile file)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);
        await _http.PostAsync("Documents/upload", content);
    }
}
