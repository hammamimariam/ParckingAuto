using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.DTO;
using ParckingAuto.Services;
using AutoMapper;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur")]
    public class SettingsController : ControllerBase
    {
        private readonly ParametresService _service;
        private readonly IMapper _mapper;

        public SettingsController(ParametresService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ParametresDto>> Get()
        {
            var parametres = await _service.GetAsync();
            return Ok(_mapper.Map<ParametresDto>(parametres));
        }

        [HttpPut("update")]
        public async Task<ActionResult<ParametresDto>> Update(ParametresDto dto)
        {
            var updated = await _service.UpdateAsync(dto);
            return Ok(_mapper.Map<ParametresDto>(updated));
        }
    }
}
