using ParckingAuto.Enums;
using ParckingAuto.Models;

namespace ParckingAuto.Seed;

public static class SampleData
{
    public static List<Vehicule> GetSampleVehicles() =>
    [
        new Vehicule
        {
            Immatriculation = "123 TUN 456",
            Marque = "Peugeot",
            Modele = "308",
            Constructeur = "Peugeot",
            AnneeFabrication = 2020,
            AnneeMiseEnCirculation = 2020,
            TypeCarburant = TypeCarburantEnum.Diesel,
            Kilometrage = 45000,
            DernierKmVidange = 40000,
            NumeroChassis = "VF33C8HR8KS123456",
            AssuranceDate = DateTime.Today.AddMonths(3),
            CompagnieAssurance = "AXA Tunisie",
            ProchaineVisite = DateTime.Today.AddMonths(6),
            NombrePlaces = 5,
            GenreVehicule = "Voiture de tourisme",
            Couleur = "Gris"
        },
        new Vehicule
        {
            Immatriculation = "789 TUN 012",
            Marque = "Renault",
            Modele = "Clio",
            Constructeur = "Renault",
            AnneeFabrication = 2021,
            AnneeMiseEnCirculation = 2021,
            TypeCarburant = TypeCarburantEnum.Essence,
            Kilometrage = 28000,
            DernierKmVidange = 25000,
            NumeroChassis = "VF1R98000M1234567",
            AssuranceDate = DateTime.Today.AddMonths(5),
            CompagnieAssurance = "STAR Assurance",
            ProchaineVisite = DateTime.Today.AddMonths(8),
            NombrePlaces = 5,
            GenreVehicule = "Voiture de tourisme",
            Couleur = "Bleu"
        },
        new Vehicule
        {
            Immatriculation = "456 TUN 789",
            Marque = "Toyota",
            Modele = "Hilux",
            Constructeur = "Toyota",
            AnneeFabrication = 2019,
            AnneeMiseEnCirculation = 2019,
            TypeCarburant = TypeCarburantEnum.Diesel,
            Kilometrage = 72000,
            DernierKmVidange = 70000,
            NumeroChassis = "MR0FZ29G001234567",
            AssuranceDate = DateTime.Today.AddMonths(1),
            CompagnieAssurance = "SIAM Assurances",
            ProchaineVisite = DateTime.Today.AddMonths(3),
            NombrePlaces = 5,
            GenreVehicule = "Pick-up",
            Couleur = "Blanc"
        },
        new Vehicule
        {
            Immatriculation = "147 TUN 258",
            Marque = "Volkswagen",
            Modele = "Golf",
            Constructeur = "Volkswagen",
            AnneeFabrication = 2022,
            AnneeMiseEnCirculation = 2022,
            TypeCarburant = TypeCarburantEnum.Diesel,
            Kilometrage = 15000,
            DernierKmVidange = 10000,
            NumeroChassis = "WVWZZZAUZJW123456",
            AssuranceDate = DateTime.Today.AddMonths(10),
            CompagnieAssurance = "Comar Assurances",
            ProchaineVisite = DateTime.Today.AddMonths(12),
            NombrePlaces = 5,
            GenreVehicule = "Voiture de tourisme",
            Couleur = "Noir"
        },
        new Vehicule
        {
            Immatriculation = "369 TUN 741",
            Marque = "Ford",
            Modele = "Ranger",
            Constructeur = "Ford",
            AnneeFabrication = 2020,
            AnneeMiseEnCirculation = 2020,
            TypeCarburant = TypeCarburantEnum.Diesel,
            Kilometrage = 95000,
            DernierKmVidange = 90000,
            NumeroChassis = "1FTEX1EP2KFA12345",
            AssuranceDate = DateTime.Today.AddMonths(7),
            CompagnieAssurance = "GAT Assurances",
            ProchaineVisite = DateTime.Today.AddMonths(9),
            NombrePlaces = 5,
            GenreVehicule = "Pick-up",
            Couleur = "Rouge"
        }
    ];

    public static List<Carburant> GetSampleCarburants(List<Vehicule> vehicles) =>
    [
        new Carburant
        {
            VehiculeId = vehicles[0].Id,
            DatePlein = DateTime.Today.AddDays(-15),
            VolumeLitres = 45.5M,
            Montant = 320.00M,
            Kilometrage = 42000
        },
        new Carburant
        {
            VehiculeId = vehicles[0].Id,
            DatePlein = DateTime.Today.AddDays(-8),
            VolumeLitres = 40.2M,
            Montant = 280.00M,
            Kilometrage = 43500
        },
        new Carburant
        {
            VehiculeId = vehicles[0].Id,
            DatePlein = DateTime.Today.AddDays(-25),
            VolumeLitres = 42.0M,
            Montant = 295.00M,
            Kilometrage = 40500
        },
        new Carburant
        {
            VehiculeId = vehicles[0].Id,
            DatePlein = DateTime.Today.AddDays(-35),
            VolumeLitres = 44.0M,
            Montant = 310.00M,
            Kilometrage = 39000
        },
        new Carburant
        {
            VehiculeId = vehicles[1].Id,
            DatePlein = DateTime.Today.AddDays(-10),
            VolumeLitres = 35.8M,
            Montant = 245.00M,
            Kilometrage = 26000
        },
        new Carburant
        {
            VehiculeId = vehicles[1].Id,
            DatePlein = DateTime.Today.AddDays(-20),
            VolumeLitres = 38.0M,
            Montant = 260.00M,
            Kilometrage = 24500
        },
        new Carburant
        {
            VehiculeId = vehicles[1].Id,
            DatePlein = DateTime.Today.AddDays(-30),
            VolumeLitres = 36.5M,
            Montant = 250.00M,
            Kilometrage = 23000
        },
        new Carburant
        {
            VehiculeId = vehicles[2].Id,
            DatePlein = DateTime.Today.AddDays(-20),
            VolumeLitres = 60.0M,
            Montant = 420.00M,
            Kilometrage = 68000
        },
        new Carburant
        {
            VehiculeId = vehicles[2].Id,
            DatePlein = DateTime.Today.AddDays(-40),
            VolumeLitres = 65.0M,
            Montant = 455.00M,
            Kilometrage = 65000
        },
        new Carburant
        {
            VehiculeId = vehicles[2].Id,
            DatePlein = DateTime.Today.AddDays(-55),
            VolumeLitres = 58.0M,
            Montant = 406.00M,
            Kilometrage = 62000
        },
        new Carburant
        {
            VehiculeId = vehicles[3].Id,
            DatePlein = DateTime.Today.AddDays(-5),
            VolumeLitres = 48.0M,
            Montant = 336.00M,
            Kilometrage = 14000
        },
        new Carburant
        {
            VehiculeId = vehicles[3].Id,
            DatePlein = DateTime.Today.AddDays(-18),
            VolumeLitres = 50.0M,
            Montant = 350.00M,
            Kilometrage = 12500
        },
        new Carburant
        {
            VehiculeId = vehicles[4].Id,
            DatePlein = DateTime.Today.AddDays(-12),
            VolumeLitres = 70.0M,
            Montant = 490.00M,
            Kilometrage = 92000
        },
        new Carburant
        {
            VehiculeId = vehicles[4].Id,
            DatePlein = DateTime.Today.AddDays(-30),
            VolumeLitres = 72.0M,
            Montant = 504.00M,
            Kilometrage = 88000
        },
        new Carburant
        {
            VehiculeId = vehicles[4].Id,
            DatePlein = DateTime.Today.AddDays(-45),
            VolumeLitres = 68.0M,
            Montant = 476.00M,
            Kilometrage = 84000
        }
    ];

    public static List<Maintenance> GetSampleMaintenances(List<Vehicule> vehicles) =>
    [
        new Maintenance
        {
            VehiculeId = vehicles[0].Id,
            DateIntervention = DateTime.Today.AddMonths(-2),
            TypeIntervention = "Vidange d'huile",
            Description = "Remplacement huile moteur et filtre",
            Cout = 180.00M,
            KilometrageIntervention = 40000
        },
        new Maintenance
        {
            VehiculeId = vehicles[0].Id,
            DateIntervention = DateTime.Today.AddDays(-5),
            TypeIntervention = "Pneus",
            Description = "Remplacement des 2 pneus avant",
            Cout = 250.00M,
            KilometrageIntervention = 44000
        },
        new Maintenance
        {
            VehiculeId = vehicles[0].Id,
            DateIntervention = DateTime.Today.AddMonths(-4),
            TypeIntervention = "Freins",
            Description = "Remplacement plaquettes freins",
            Cout = 120.00M,
            KilometrageIntervention = 38000
        },
        new Maintenance
        {
            VehiculeId = vehicles[1].Id,
            DateIntervention = DateTime.Today.AddMonths(-1),
            TypeIntervention = "Freins",
            Description = "Remplacement plaquettes freins avant",
            Cout = 120.00M,
            KilometrageIntervention = 25000
        },
        new Maintenance
        {
            VehiculeId = vehicles[1].Id,
            DateIntervention = DateTime.Today.AddMonths(-3),
            TypeIntervention = "Vidange d'huile",
            Description = "Vidange et filtre",
            Cout = 170.00M,
            KilometrageIntervention = 23000
        },
        new Maintenance
        {
            VehiculeId = vehicles[2].Id,
            DateIntervention = DateTime.Today.AddMonths(-3),
            TypeIntervention = "Vidange d'huile",
            Description = "Remplacement huile moteur et filtre",
            Cout = 220.00M,
            KilometrageIntervention = 65000
        },
        new Maintenance
        {
            VehiculeId = vehicles[2].Id,
            DateIntervention = DateTime.Today.AddMonths(-5),
            TypeIntervention = "Filtre air",
            Description = "Remplacement filtre à air",
            Cout = 50.00M,
            KilometrageIntervention = 63000
        },
        new Maintenance
        {
            VehiculeId = vehicles[2].Id,
            DateIntervention = DateTime.Today.AddMonths(-7),
            TypeIntervention = "Pneus",
            Description = "Remplacement des 4 pneus",
            Cout = 480.00M,
            KilometrageIntervention = 60000
        },
        new Maintenance
        {
            VehiculeId = vehicles[3].Id,
            DateIntervention = DateTime.Today.AddMonths(-1),
            TypeIntervention = "Vidange d'huile",
            Description = "Vidange et filtre",
            Cout = 200.00M,
            KilometrageIntervention = 10000
        },
        new Maintenance
        {
            VehiculeId = vehicles[3].Id,
            DateIntervention = DateTime.Today.AddDays(-10),
            TypeIntervention = "Climatisation",
            Description = "Recharge climatisation",
            Cout = 80.00M,
            KilometrageIntervention = 13000
        },
        new Maintenance
        {
            VehiculeId = vehicles[4].Id,
            DateIntervention = DateTime.Today.AddMonths(-2),
            TypeIntervention = "Vidange d'huile",
            Description = "Vidange et filtre",
            Cout = 230.00M,
            KilometrageIntervention = 90000
        },
        new Maintenance
        {
            VehiculeId = vehicles[4].Id,
            DateIntervention = DateTime.Today.AddMonths(-4),
            TypeIntervention = "Pneus",
            Description = "Remplacement des 2 pneus arrière",
            Cout = 260.00M,
            KilometrageIntervention = 86000
        }
    ];

    public static List<Mouvement> GetSampleMouvements(List<Vehicule> vehicles, List<Chauffeur> chauffeurs) =>
    [
        new Mouvement
        {
            VehiculeId = vehicles[0].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today.AddDays(-10),
            DateRetour = DateTime.Today.AddDays(-7),
            KmDepart = 43000,
            KmRetour = 43800,
            Destination = "Sfax"
        },
        new Mouvement
        {
            VehiculeId = vehicles[0].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today.AddDays(-20),
            DateRetour = DateTime.Today.AddDays(-18),
            KmDepart = 42000,
            KmRetour = 42500,
            Destination = "Bizerte"
        },
        new Mouvement
        {
            VehiculeId = vehicles[1].Id,
            ChauffeurId = chauffeurs[1].Id,
            DateDepart = DateTime.Today.AddDays(-3),
            DateRetour = DateTime.Today.AddDays(-1),
            KmDepart = 27000,
            KmRetour = 27300,
            Destination = "Hammamet"
        },
        new Mouvement
        {
            VehiculeId = vehicles[1].Id,
            ChauffeurId = chauffeurs[1].Id,
            DateDepart = DateTime.Today.AddDays(-15),
            DateRetour = DateTime.Today.AddDays(-12),
            KmDepart = 25500,
            KmRetour = 26200,
            Destination = "Nabeul"
        },
        new Mouvement
        {
            VehiculeId = vehicles[2].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today,
            KmDepart = 71000,
            Destination = "Gabès"
        },
        new Mouvement
        {
            VehiculeId = vehicles[2].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today.AddDays(-25),
            DateRetour = DateTime.Today.AddDays(-22),
            KmDepart = 68500,
            KmRetour = 69500,
            Destination = "Medenine"
        },
        new Mouvement
        {
            VehiculeId = vehicles[3].Id,
            ChauffeurId = chauffeurs[1].Id,
            DateDepart = DateTime.Today.AddDays(-8),
            DateRetour = DateTime.Today.AddDays(-6),
            KmDepart = 13200,
            KmRetour = 13800,
            Destination = "Carthage"
        },
        new Mouvement
        {
            VehiculeId = vehicles[3].Id,
            ChauffeurId = chauffeurs[1].Id,
            DateDepart = DateTime.Today.AddDays(-18),
            DateRetour = DateTime.Today.AddDays(-15),
            KmDepart = 12000,
            KmRetour = 12800,
            Destination = "Sousse"
        },
        new Mouvement
        {
            VehiculeId = vehicles[4].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today.AddDays(-12),
            DateRetour = DateTime.Today.AddDays(-9),
            KmDepart = 93000,
            KmRetour = 94000,
            Destination = "Kebili"
        },
        new Mouvement
        {
            VehiculeId = vehicles[4].Id,
            ChauffeurId = chauffeurs[0].Id,
            DateDepart = DateTime.Today.AddDays(-28),
            DateRetour = DateTime.Today.AddDays(-25),
            KmDepart = 90500,
            KmRetour = 91800,
            Destination = "Tozeur"
        }
    ];

    public static List<Alerte> GetSampleAlertes(List<Vehicule> vehicles) =>
    [
        new Alerte
        {
            VehiculeId = vehicles[2].Id,
            TypeAlerte = TypeAlerteEnum.Assurance,
            Message = "Assurance expire dans 1 mois",
            Statut = StatutAlerteEnum.Critique,
            DateAlerte = DateTime.Today
        },
        new Alerte
        {
            VehiculeId = vehicles[0].Id,
            TypeAlerte = TypeAlerteEnum.Vidange,
            Message = "Vidange due dans 3000 km",
            Statut = StatutAlerteEnum.PreAlerte,
            DateAlerte = DateTime.Today.AddDays(-2)
        },
        new Alerte
        {
            VehiculeId = vehicles[4].Id,
            TypeAlerte = TypeAlerteEnum.Vidange,
            Message = "Vidange due dans 5000 km",
            Statut = StatutAlerteEnum.PreAlerte,
            DateAlerte = DateTime.Today.AddDays(-5)
        },
        new Alerte
        {
            VehiculeId = vehicles[1].Id,
            TypeAlerte = TypeAlerteEnum.VisiteTechnique,
            Message = "Visite technique due dans 8 mois",
            Statut = StatutAlerteEnum.PreAlerte,
            DateAlerte = DateTime.Today.AddDays(-1)
        },
        new Alerte
        {
            VehiculeId = vehicles[3].Id,
            TypeAlerte = TypeAlerteEnum.Assurance,
            Message = "Assurance expire dans 10 mois",
            Statut = StatutAlerteEnum.PreAlerte,
            DateAlerte = DateTime.Today.AddDays(-3)
        }
    ];
}
