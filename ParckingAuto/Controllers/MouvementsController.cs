using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MouvementsController : ControllerBase
    {
        private readonly MouvementService _service;
        private readonly IMapper _mapper;

        public MouvementsController(MouvementService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MouvementDto>>> GetAll()
        {
            var mouvements = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<MouvementDto>>(mouvements));
        }

        [HttpPost]
        [Authorize(Roles = "Chauffeur,Gestionnaire,Administrateur")]
        public async Task<ActionResult<MouvementDto>> Add(MouvementDto dto)
        {
            try
            {
                var entity = _mapper.Map<Models.Mouvement>(dto);
                var created = await _service.AddAsync(entity);
                return Ok(_mapper.Map<MouvementDto>(created));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Chauffeur,Gestionnaire,Administrateur")]
        public async Task<IActionResult> Update(int id, MouvementDto dto)
        {
            if (id != dto.Id) return BadRequest();
            try
            {
                var entity = _mapper.Map<Models.Mouvement>(dto);
                await _service.UpdateAsync(entity);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
