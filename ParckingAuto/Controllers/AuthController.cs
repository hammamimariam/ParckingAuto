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

        public AuthController(JwtService jwtService, UtilisateurService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = (await _userService.GetAllAsync())
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null || !PasswordHasher.Verify(request.Password, user.MotDePasse))
                return Unauthorized(new { message = "Email ou mot de passe incorrect" });

            if (!user.MotDePasse.StartsWith("$2"))
            {
                user.MotDePasse = PasswordHasher.Hash(request.Password);
                await _userService.UpdateAsync(user);
            }

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
            return Ok(new { message = "Mot de passe réinitialisé avec succès" });
        }
    }
}
