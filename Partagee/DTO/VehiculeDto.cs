using System;

namespace ParckingAuto.DTO
{
    public class VehiculeDto
    {
        public int Id { get; set; }
        public string Immatriculation { get; set; } = string.Empty;
        public string Marque { get; set; } = string.Empty;
        public string Constructeur { get; set; } = string.Empty;
        public string Modele { get; set; } = string.Empty;
        public int AnneeFabrication { get; set; }
        public int AnneeMiseEnCirculation { get; set; }
        public string TypeCarburant { get; set; } = string.Empty;
        public string NumeroChassis { get; set; } = string.Empty;
        public int Kilometrage { get; set; }
        public int DernierKmVidange { get; set; }
        public int KmDepuisVidange { get; set; }

        // Carte grise tunisienne
        public string NumeroCarteGrise { get; set; } = string.Empty;
        public string GenreVehicule { get; set; } = string.Empty;
        public int PuissanceFiscale { get; set; }
        public int NombrePlaces { get; set; }
        public string Couleur { get; set; } = string.Empty;
        public DateTime? DatePremiereMiseEnCirculation { get; set; }

        // Attestation d'assurance tunisienne
        public string Assurance { get; set; } = string.Empty;
        public string AssuranceReference { get; set; } = string.Empty;
        public DateTime? AssuranceDateDebut { get; set; }
        public DateTime? AssuranceDate { get; set; }
        public string UsageVehicule { get; set; } = string.Empty;

        // Visite technique
        public DateTime? VisiteTechniqueDate { get; set; }
        public DateTime? ProchaineVisite { get; set; }
        public string Statut { get; set; } = "Au parking";
    }
}
