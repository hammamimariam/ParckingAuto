using System;
using System.Collections.Generic;

namespace ParckingAuto.DTO
{
    public class DashboardChartsDto
    {
        public List<string> Mois { get; set; } = new();
        public List<double> LitresParMois { get; set; } = new();
        public List<double> CoutCarburantParMois { get; set; } = new();
        public List<double> CoutMaintenanceParMois { get; set; } = new();
        public int VehiculesEnMission { get; set; }
        public int VehiculesAuParking { get; set; }
        public double CoutMaintenanceTotal { get; set; }
        public double CoutMaintenance6Mois { get; set; }
        public double CoutCarburant6Mois { get; set; }
        public double ConsommationCarburantTotal { get; set; }
    }
}
