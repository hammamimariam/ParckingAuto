using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceService _service;
        private readonly IMapper _mapper;

        public MaintenanceController(MaintenanceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceDto>>> GetAll()
        {
            var maintenances = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<MaintenanceDto>>(maintenances));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceDto>> GetById(int id)
        {
            var m = await _service.GetByIdAsync(id);
            if (m == null) return NotFound();
            return Ok(_mapper.Map<MaintenanceDto>(m));
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceDto>> Add(MaintenanceDto dto)
        {
            var entity = _mapper.Map<Models.Maintenance>(dto);
            var created = await _service.AddAsync(entity);
            return Ok(_mapper.Map<MaintenanceDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MaintenanceDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = _mapper.Map<Models.Maintenance>(dto);
            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
