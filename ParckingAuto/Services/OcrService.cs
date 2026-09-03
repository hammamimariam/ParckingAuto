namespace ParckingAuto.Services;

public class OcrService : IOcrService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private const string OcrApiUrl = "https://api.ocr.space/parse/image";

    public OcrService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> ExtractTextFromImageAsync(IFormFile file)
    {
        var apiKey = _configuration["Ocr:ApiKey"] ?? "K82932178388957"; // Use user provided key as fallback
        
        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        content.Add(new StreamContent(fileStream), "file", file.FileName);
        content.Add(new StringContent(apiKey), "apikey");
        content.Add(new StringContent("fre"), "language"); // French, you can change to "eng" if needed
        content.Add(new StringContent("true"), "isOverlayRequired");
        content.Add(new StringContent("true"), "detectOrientation");
        content.Add(new StringContent("true"), "scale");

        var response = await _httpClient.PostAsync(OcrApiUrl, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Parse OCR.space response to get extracted text
        var ocrResult = System.Text.Json.JsonDocument.Parse(jsonResponse);
        var parsedResults = ocrResult.RootElement.GetProperty("ParsedResults");
        
        if (parsedResults.GetArrayLength() > 0)
        {
            var parsedText = parsedResults[0].GetProperty("ParsedText");
            return parsedText.GetString() ?? "";
        }

        return "";
    }
}
