using ParckingAuto.Enums;

namespace ParckingAuto.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }   // Enum au lieu de string
    }
}
