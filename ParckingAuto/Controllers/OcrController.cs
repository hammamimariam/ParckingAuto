using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;

namespace ParckingAuto.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OcrController : ControllerBase
{
    private readonly IOcrService _ocrService;

    public OcrController(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    [HttpPost("scan")]
    public async Task<ActionResult<string>> ScanDocument(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        try
        {
            var extractedText = await _ocrService.ExtractTextFromImageAsync(file);
            return Ok(extractedText);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during OCR scan: {ex.Message}");
        }
    }
}
