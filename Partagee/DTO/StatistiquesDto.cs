using System;
using System.Collections.Generic;

namespace ParckingAuto.DTO
{
    public class StatistiquesDto
    {
        public List<string> Mois { get; set; } = new();
        public List<decimal> CoutMaintenanceParMois { get; set; } = new();
        public List<decimal> CoutCarburantParMois { get; set; } = new();
        public List<double> LitresParMois { get; set; } = new();
        public List<double> ConsommationMoyenneParMois { get; set; } = new();
        public List<ConsommationVehiculeDto> ConsommationParVehicule { get; set; } = new();
    }

    public class ConsommationVehiculeDto
    {
        public string Immatriculation { get; set; } = string.Empty;
        public double ConsommationMoyenne { get; set; }
    }
}
