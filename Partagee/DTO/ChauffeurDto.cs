using System;

namespace ParckingAuto.DTO
{
    public class ChauffeurDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string PermisNumero { get; set; } = string.Empty;
        public DateTime PermisExpiration { get; set; }
        public string Statut { get; set; } = "Disponible";
        public int? UtilisateurId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
    }
}
