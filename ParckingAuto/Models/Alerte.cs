using ParckingAuto.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParckingAuto.Models
{
    public class Alerte
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule? Vehicule { get; set; }

        public TypeAlerteEnum TypeAlerte { get; set; }   // Enum
        public DateTime DateAlerte { get; set; }
        public StatutAlerteEnum Statut { get; set; } = StatutAlerteEnum.PreAlerte;   // Enum
        public DateTime? DateResolution { get; set; }
        public string ReferenceDeclencheur { get; set; } = string.Empty;

        [NotMapped]
        public string VehiculeImmatriculation { get; set; } = string.Empty;

        [NotMapped]
        public string Message { get; set; } = string.Empty;
    }
}
