using ParckingAuto.Enums;

namespace ParckingAuto.Models
{
    public class Vehicule
    {
        public int Id { get; set; }
        public string Immatriculation { get; set; } = string.Empty;
        public string Marque { get; set; } = string.Empty;
        public string Constructeur { get; set; } = string.Empty;
        public string Modele { get; set; } = string.Empty;
        public int AnneeFabrication { get; set; }
        public int AnneeMiseEnCirculation { get; set; }
        public TypeCarburantEnum TypeCarburant { get; set; }
        public string NumeroChassis { get; set; } = string.Empty;
        public int Kilometrage { get; set; }
        public int DernierKmVidange { get; set; }

        // Carte grise tunisienne
        public string NumeroCarteGrise { get; set; } = string.Empty;
        public string GenreVehicule { get; set; } = string.Empty;
        public int PuissanceFiscale { get; set; }
        public int NombrePlaces { get; set; }
        public string Couleur { get; set; } = string.Empty;
        public DateTime? DatePremiereMiseEnCirculation { get; set; }

        // Attestation d'assurance tunisienne
        public string CompagnieAssurance { get; set; } = string.Empty;
        public string AssuranceReference { get; set; } = string.Empty;
        public DateTime? AssuranceDateDebut { get; set; }
        public DateTime? AssuranceDate { get; set; }
        public string UsageVehicule { get; set; } = string.Empty;

        // Visite technique
        public DateTime? VisiteTechniqueDate { get; set; }
        public DateTime? ProchaineVisite { get; set; }
    }
}
