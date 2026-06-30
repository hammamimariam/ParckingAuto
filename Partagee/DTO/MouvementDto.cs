using System;

namespace ParckingAuto.DTO
{
    public class MouvementDto
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public int ChauffeurId { get; set; }
        public DateTime DateDepart { get; set; }
        public DateTime? DateRetour { get; set; }
        public string Destination { get; set; } = string.Empty;
        public int KmDepart { get; set; }
        public int? KmRetour { get; set; }
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public string ChauffeurNomComplet { get; set; } = string.Empty;
    }
}
