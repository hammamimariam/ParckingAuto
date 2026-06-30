using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Services;
using ParckingAuto.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ParckingAuto.Enums;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilisateursController : ControllerBase
    {
        private readonly UtilisateurService _service;
        private readonly IMapper _mapper;

        public UtilisateursController(UtilisateurService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrateur,Gestionnaire")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpPost("register")]
        [Authorize(Roles = "Administrateur")]
        public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
        {
            var user = new Models.Utilisateur
            {
                Nom = request.Nom,
                Email = request.Email,
                MotDePasse = PasswordHasher.Hash(request.MotDePasse),
                Role = Enum.Parse<RoleEnum>(request.Role, ignoreCase: true)
            };
            var created = await _service.AddAsync(user);
            return Ok(_mapper.Map<UserDto>(created));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Collaborateur introuvable." });

            user.Nom = request.Nom;
            user.Email = request.Email;
            user.Role = Enum.Parse<RoleEnum>(request.Role, ignoreCase: true);

            if (!string.IsNullOrWhiteSpace(request.MotDePasse))
                user.MotDePasse = PasswordHasher.Hash(request.MotDePasse);

            await _service.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Collaborateur introuvable." });

            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Impossible de supprimer ce collaborateur. Il est peut-être lié à un chauffeur." });
            }
        }
    }
}
