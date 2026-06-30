using System;

namespace ParckingAuto.DTO
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public string TypeIntervention { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateIntervention { get; set; }
        public int KilometrageIntervention { get; set; }
        public decimal Cout { get; set; }
        public string Fournisseur { get; set; } = string.Empty;
        public string Facture { get; set; } = string.Empty;
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public string VehiculeMarque { get; set; } = string.Empty;
        public string VehiculeModele { get; set; } = string.Empty;
    }
}
