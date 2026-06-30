using System;
using System.Collections.Generic;

namespace ParckingAuto.DTO
{
    public class VehiculeSuiviDto
    {
        public VehiculeDto Vehicule { get; set; } = new();
        public int KmDepuisVidange { get; set; }
        public decimal CoutMaintenanceMoisCourant { get; set; }
        public decimal CoutCarburantMoisCourant { get; set; }
        public double LitresMoisCourant { get; set; }
        public double ConsommationMoyenne { get; set; }
        public decimal CoutMaintenanceTotal { get; set; }
        public decimal CoutCarburantTotal { get; set; }
        public List<MouvementDto> DerniersMouvements { get; set; } = new();
        public List<CarburantDto> DerniersPleins { get; set; } = new();
        public List<MaintenanceDto> DernieresMaintenances { get; set; } = new();
        public List<AlerteDto> AlertesActives { get; set; } = new();
    }
}
