using Microsoft.AspNetCore.Mvc;
using ParckingAuto.DTO;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Services;

namespace ParckingAuto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UtilisateurService _userService;
        private readonly AuditService _auditService;

        public AuthController(JwtService jwtService, UtilisateurService userService, AuditService auditService)
        {
            _jwtService = jwtService;
            _userService = userService;
            _auditService = auditService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = (await _userService.GetAllAsync())
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null || !PasswordHasher.Verify(request.Password, user.MotDePasse))
            {
                await _auditService.LogActionAsync(null, "LoginFailed", "Auth", null, null, new { Email = request.Email });
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });
            }

            if (!user.MotDePasse.StartsWith("$2"))
            {
                user.MotDePasse = PasswordHasher.Hash(request.Password);
                await _userService.UpdateAsync(user);
            }

            await _auditService.LogActionAsync(user.Id, "Login", "Auth", null, null, new { user.Email, user.Role });

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Role.ToString());

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Nom = user.Nom,
                Email = user.Email,
                Role = user.Role.ToString()
            });
        }

        [HttpPost("register")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new Utilisateur
            {
                Nom = request.Nom,
                Email = request.Email,
                MotDePasse = PasswordHasher.Hash(request.MotDePasse),
                Role = Enum.Parse<RoleEnum>(request.Role, true)
            };

            await _userService.AddAsync(user);

            // Get the current user from the JWT token
            var currentUserId = int.Parse(User.Identity?.Name ?? "0");
            await _auditService.LogActionAsync(currentUserId, "Create", "Utilisateurs", user.Id.ToString(), null, new { user.Nom, user.Email, user.Role });

            return Ok(new { message = "Utilisateur créé avec succès" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = (await _userService.GetAllAsync())
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable" });

            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
                return BadRequest(new { message = "Le mot de passe doit contenir au moins 6 caractères" });

            user.MotDePasse = PasswordHasher.Hash(request.NewPassword);
            await _userService.UpdateAsync(user);

            await _auditService.LogActionAsync(null, "ResetPassword", "Utilisateurs", user.Id.ToString(), null, new { user.Email });

            return Ok(new { message = "Mot de passe réinitialisé avec succès" });
        }
    }
}
