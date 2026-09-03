using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParckingAuto.Models;
using ParckingAuto.Services;
using AutoMapper;
using ParckingAuto.DTO;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrateur")]
    public class AuditController : ControllerBase
    {
        private readonly AuditService _auditService;
        private readonly IMapper _mapper;
        public AuditController(AuditService auditService, IMapper mapper)
        {
            _auditService = auditService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditService.GetAllAsync();
            var dtos = _mapper.Map<List<AuditDto>>(logs);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _auditService.GetByIdAsync(id);
            if (log == null) return NotFound();
            var dto = _mapper.Map<AuditDto>(log);
            return Ok(dto);
        }
    }
}
