using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentService _service;
        private readonly IMapper _mapper;

        public DocumentsController(DocumentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAll()
        {
            var docs = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<DocumentDto>>(docs));
        }

        [HttpPost]
        public async Task<ActionResult<DocumentDto>> Add(DocumentDto dto)
        {
            var entity = _mapper.Map<Models.Document>(dto);
            var created = await _service.AddAsync(entity);
            return Ok(_mapper.Map<DocumentDto>(created));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            return await SaveUploadedFile(file);
        }

        private async Task<IActionResult> SaveUploadedFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fichier vide");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = $"/uploads/{uniqueFileName}";
            return Ok(new { url = fileUrl, fileName = file.FileName });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
