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
        private readonly AuditService _auditService;

        public ChauffeursController(ChauffeurService service, IMapper mapper, AuditService auditService)
        {
            _service = service;
            _mapper = mapper;
            _auditService = auditService;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("name")?.Value;
            return string.IsNullOrEmpty(userIdClaim) ? null : int.Parse(userIdClaim);
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
                
                await _auditService.LogActionAsync(GetCurrentUserId(), "Create", "Chauffeurs", created.Id.ToString(), null, new
                {
                    created.Prenom,
                    created.Nom,
                    created.PermisNumero
                });
                
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
                var old = await _service.GetByIdAsync(id);
                
                var entity = _mapper.Map<Models.Chauffeur>(dto);
                await _service.UpdateAsync(entity, dto.Email, dto.MotDePasse);
                
                await _auditService.LogActionAsync(GetCurrentUserId(), "Update", "Chauffeurs", id.ToString(), new
                {
                    old?.Prenom,
                    old?.Nom,
                    old?.PermisNumero
                }, new
                {
                    entity.Prenom,
                    entity.Nom,
                    entity.PermisNumero
                });
                
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
                var old = await _service.GetByIdAsync(id);
                await _service.DeleteAsync(id);
                
                await _auditService.LogActionAsync(GetCurrentUserId(), "Delete", "Chauffeurs", id.ToString(), new
                {
                    old?.Prenom,
                    old?.Nom,
                    old?.PermisNumero
                }, null);
                
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Impossible de supprimer ce chauffeur : des mouvements y sont encore associés.");
            }
        }
    }
}
