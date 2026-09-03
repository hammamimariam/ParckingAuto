namespace Front.Services;

public class OcrService
{
    private readonly HttpClient _httpClient;

    public OcrService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ScanDocumentAsync(MultipartFormDataContent content)
    {
        var response = await _httpClient.PostAsync("Ocr/scan", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
