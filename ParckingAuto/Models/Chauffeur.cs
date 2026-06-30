using ParckingAuto.Enums;

namespace ParckingAuto.Models
{
    public class Chauffeur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string PermisNumero { get; set; } = string.Empty;
        public DateTime PermisExpiration { get; set; }
        public StatutChauffeurEnum Statut { get; set; } = StatutChauffeurEnum.Disponible;

        public int? UtilisateurId { get; set; }
        public Utilisateur? Utilisateur { get; set; }
    }
}
