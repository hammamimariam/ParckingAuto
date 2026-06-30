using System;

namespace ParckingAuto.DTO
{
    public class AlerteDto
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public string TypeAlerte { get; set; } = string.Empty;
        public DateTime DateAlerte { get; set; }
        public string Statut { get; set; } = string.Empty;
        public DateTime? DateResolution { get; set; }
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
