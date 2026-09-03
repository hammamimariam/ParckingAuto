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

    public class StatistiquesFilterDto
    {
        public int? Annee { get; set; }
        public int? Mois { get; set; }
        public int? VehiculeId { get; set; }
        public int? ChauffeurId { get; set; }
    }

    public class CarburantExportDto
    {
        public DateTime DatePlein { get; set; }
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public double Litres { get; set; }
        public double Cout { get; set; }
        public double Kilometrage { get; set; }
    }

    public class MaintenanceExportDto
    {
        public DateTime DateIntervention { get; set; }
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public string TypeIntervention { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cout { get; set; }
    }

    public class MouvementExportDto
    {
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string VehiculeImmatriculation { get; set; } = string.Empty;
        public string ChauffeurNomComplet { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
