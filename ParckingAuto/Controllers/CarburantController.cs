using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarburantController : ControllerBase
    {
        private readonly CarburantService _service;
        private readonly IMapper _mapper;

        public CarburantController(CarburantService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarburantDto>>> GetAll()
        {
            var carburants = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CarburantDto>>(carburants));
        }

        [Authorize(Roles = "Administrateur,Gestionnaire")]
        [HttpPost]
        public async Task<ActionResult<CarburantDto>> Add(CarburantDto dto)
        {
            var entity = _mapper.Map<Models.Carburant>(dto);
            var created = await _service.AddAsync(entity);
            return Ok(_mapper.Map<CarburantDto>(created));
        }

        [Authorize(Roles = "Administrateur,Gestionnaire")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
