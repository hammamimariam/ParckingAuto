namespace ParckingAuto.Services;

public interface IOcrService
{
    Task<string> ExtractTextFromImageAsync(IFormFile file);
}
