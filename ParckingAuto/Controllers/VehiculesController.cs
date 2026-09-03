using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.DTO;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Services;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculesController : ControllerBase
    {
        private readonly VehiculeService _service;
        private readonly IMapper _mapper;
        private readonly AuditService _auditService;

        public VehiculesController(VehiculeService service, IMapper mapper, AuditService auditService)
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
        public async Task<ActionResult<IEnumerable<VehiculeDto>>> GetAll()
        {
            var vehicules = await _service.GetAllAsync();
            return Ok(await _service.ToDtoListAsync(vehicules));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculeDto>> GetById(int id)
        {
            var v = await _service.GetByIdAsync(id);
            if (v == null) return NotFound();

            var dto = _mapper.Map<VehiculeDto>(v);
            dto.Statut = await _service.GetStatutAsync(id);
            return Ok(dto);
        }

        [Authorize(Roles = "Administrateur,Gestionnaire")]
        [HttpPost]
        public async Task<ActionResult<VehiculeDto>> Add(VehiculeDto dto)
        {
            var entity = _mapper.Map<Vehicule>(dto);
            var created = await _service.AddAsync(entity);
            var result = _mapper.Map<VehiculeDto>(created);
            result.Statut = "Au parking";

            await _auditService.LogActionAsync(GetCurrentUserId(), "Create", "Vehicules", created.Id.ToString(), null, new
            {
                created.Immatriculation,
                created.Marque,
                created.Modele
            });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, result);
        }

        [Authorize(Roles = "Administrateur,Gestionnaire")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehiculeDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var oldValues = new
            {
                existing.Immatriculation,
                existing.Marque,
                existing.Modele,
                existing.Kilometrage
            };

            _mapper.Map(dto, existing);
            if (existing.DernierKmVidange <= 0)
                existing.DernierKmVidange = existing.Kilometrage;

            await _service.UpdateAsync(existing);

            var newValues = new
            {
                existing.Immatriculation,
                existing.Marque,
                existing.Modele,
                existing.Kilometrage
            };

            await _auditService.LogActionAsync(GetCurrentUserId(), "Update", "Vehicules", id.ToString(), oldValues, newValues);

            return NoContent();
        }

        [HttpGet("{id}/suivi")]
        public async Task<ActionResult<VehiculeSuiviDto>> GetSuivi(int id)
        {
            var suivi = await _service.GetSuiviAsync(id);
            if (suivi == null) return NotFound();
            return Ok(suivi);
        }

        [Authorize(Roles = "Administrateur")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var oldValues = new
            {
                existing.Immatriculation,
                existing.Marque,
                existing.Modele
            };

            await _service.DeleteAsync(id);

            await _auditService.LogActionAsync(GetCurrentUserId(), "Delete", "Vehicules", id.ToString(), oldValues, null);

            return NoContent();
        }
    }
}
