using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChauffeursController : ControllerBase
    {
        private readonly ChauffeurService _service;
        private readonly IMapper _mapper;

        public ChauffeursController(ChauffeurService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChauffeurDto>>> GetAll()
        {
            var chauffeurs = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ChauffeurDto>>(chauffeurs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChauffeurDto>> GetById(int id)
        {
            var c = await _service.GetByIdAsync(id);
            if (c == null) return NotFound();
            return Ok(_mapper.Map<ChauffeurDto>(c));
        }

        [HttpPost]
        [Authorize(Roles = "Administrateur,Gestionnaire")]
        public async Task<ActionResult<ChauffeurDto>> Add(ChauffeurDto dto)
        {
            try
            {
                var entity = _mapper.Map<Models.Chauffeur>(dto);
                var created = await _service.AddAsync(entity, dto.Email, dto.MotDePasse);
                var result = await _service.GetByIdAsync(created.Id);
                return Ok(_mapper.Map<ChauffeurDto>(result));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrateur,Gestionnaire")]
        public async Task<IActionResult> Update(int id, ChauffeurDto dto)
        {
            if (id != dto.Id) return BadRequest();
            try
            {
                var entity = _mapper.Map<Models.Chauffeur>(dto);
                await _service.UpdateAsync(entity, dto.Email, dto.MotDePasse);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Impossible de supprimer ce chauffeur : des mouvements y sont encore associés.");
            }
        }
    }
}
