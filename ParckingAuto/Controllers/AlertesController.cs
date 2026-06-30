using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.DTO;
using ParckingAuto.Services;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur,Gestionnaire")]
    public class AlertesController : ControllerBase
    {
        private readonly AlerteService _service;
        private readonly IMapper _mapper;

        public AlertesController(AlerteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlerteDto>>> GetAll([FromQuery] bool includeResolues = false)
        {
            var alertes = await _service.GetAllAsync(includeResolues);
            return Ok(_mapper.Map<IEnumerable<AlerteDto>>(alertes));
        }

        [HttpPut("{id}/resoudre")]
        public async Task<ActionResult<AlerteDto>> Resoudre(int id)
        {
            var alerte = await _service.ResoudreAsync(id);
            if (alerte == null) return NotFound();
            return Ok(_mapper.Map<AlerteDto>(alerte));
        }

        [HttpPost]
        public async Task<ActionResult<AlerteDto>> Add(AlerteDto dto)
        {
            var entity = _mapper.Map<Models.Alerte>(dto);
            var created = await _service.AddAsync(entity);
            return Ok(_mapper.Map<AlerteDto>(created));
        }
    }
}
