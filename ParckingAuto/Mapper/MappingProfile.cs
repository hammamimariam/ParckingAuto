using AutoMapper;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Utilisateur, UserDto>().ReverseMap();

        CreateMap<Vehicule, VehiculeDto>()
            .ForMember(dest => dest.TypeCarburant, opt => opt.MapFrom(src => src.TypeCarburant.ToString()))
            .ForMember(dest => dest.Constructeur, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Constructeur) ? src.Marque : src.Constructeur))
            .ForMember(dest => dest.Assurance, opt => opt.MapFrom(src => src.CompagnieAssurance))
            .ForMember(dest => dest.KmDepuisVidange, opt => opt.MapFrom(src => Math.Max(0, src.Kilometrage - src.DernierKmVidange)))
            .ForMember(dest => dest.Statut, opt => opt.Ignore());

        CreateMap<VehiculeDto, Vehicule>()
            .ForMember(dest => dest.TypeCarburant, opt => opt.MapFrom(src => ParseCarburant(src.TypeCarburant)))
            .ForMember(dest => dest.Constructeur, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Constructeur) ? src.Marque : src.Constructeur))
            .ForMember(dest => dest.DernierKmVidange, opt => opt.MapFrom(src => src.DernierKmVidange))
            .ForMember(dest => dest.NumeroChassis, opt => opt.MapFrom(src => src.NumeroChassis ?? string.Empty))
            .ForMember(dest => dest.AnneeMiseEnCirculation, opt => opt.MapFrom(src => src.AnneeMiseEnCirculation > 0 ? src.AnneeMiseEnCirculation : src.AnneeFabrication))
            .ForMember(dest => dest.CompagnieAssurance, opt => opt.MapFrom(src => src.Assurance ?? string.Empty))
            .ForMember(dest => dest.Immatriculation, opt => opt.MapFrom(src => NormalizeImmatriculation(src.Immatriculation)));

        CreateMap<Chauffeur, ChauffeurDto>()
            .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => src.Statut.ToString()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Utilisateur != null ? src.Utilisateur.Email : ""))
            .ForMember(dest => dest.MotDePasse, opt => opt.Ignore());
        CreateMap<ChauffeurDto, Chauffeur>()
            .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => Enum.Parse<StatutChauffeurEnum>(src.Statut, true)))
            .ForMember(dest => dest.UtilisateurId, opt => opt.MapFrom(src =>
                src.UtilisateurId.HasValue && src.UtilisateurId.Value > 0 ? src.UtilisateurId : null))
            .ForMember(dest => dest.Utilisateur, opt => opt.Ignore());

        CreateMap<Mouvement, MouvementDto>()
            .ForMember(dest => dest.VehiculeImmatriculation, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Immatriculation : ""))
            .ForMember(dest => dest.ChauffeurNomComplet, opt => opt.MapFrom(src => src.Chauffeur != null ? $"{src.Chauffeur.Prenom} {src.Chauffeur.Nom}" : ""));

        CreateMap<MouvementDto, Mouvement>()
            .ForMember(dest => dest.Vehicule, opt => opt.Ignore())
            .ForMember(dest => dest.Chauffeur, opt => opt.Ignore());

        CreateMap<Carburant, CarburantDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DatePlein))
            .ForMember(dest => dest.Litres, opt => opt.MapFrom(src => (double)src.VolumeLitres))
            .ForMember(dest => dest.Cout, opt => opt.MapFrom(src => (double)src.Montant))
            .ForMember(dest => dest.Kilometrage, opt => opt.MapFrom(src => (double)src.Kilometrage))
            .ForMember(dest => dest.VehiculeImmatriculation, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Immatriculation : ""));

        CreateMap<CarburantDto, Carburant>()
            .ForMember(dest => dest.DatePlein, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.VolumeLitres, opt => opt.MapFrom(src => (decimal)src.Litres))
            .ForMember(dest => dest.Montant, opt => opt.MapFrom(src => (decimal)src.Cout))
            .ForMember(dest => dest.Kilometrage, opt => opt.MapFrom(src => (int)src.Kilometrage));

        CreateMap<Maintenance, MaintenanceDto>()
            .ForMember(dest => dest.VehiculeImmatriculation, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Immatriculation : ""))
            .ForMember(dest => dest.VehiculeMarque, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Marque : ""))
            .ForMember(dest => dest.VehiculeModele, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Modele : ""));

        CreateMap<MaintenanceDto, Maintenance>()
            .ForMember(dest => dest.Vehicule, opt => opt.Ignore());

        CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.VehiculeImmatriculation, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Immatriculation : ""))
            .ReverseMap();

        CreateMap<Alerte, AlerteDto>()
            .ForMember(dest => dest.VehiculeImmatriculation, opt => opt.MapFrom(src => src.Vehicule != null ? src.Vehicule.Immatriculation : ""))
            .ForMember(dest => dest.TypeAlerte, opt => opt.MapFrom(src => src.TypeAlerte.ToString()))
            .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => src.Statut.ToString()));

        CreateMap<Parametres, ParametresDto>().ReverseMap();
    }

    private static TypeCarburantEnum ParseCarburant(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return TypeCarburantEnum.Diesel;

        var normalized = value.Trim()
            .Replace("é", "e", StringComparison.OrdinalIgnoreCase)
            .Replace("É", "e", StringComparison.OrdinalIgnoreCase);

        if (Enum.TryParse<TypeCarburantEnum>(normalized, true, out var result))
            return result;

        return normalized.ToUpperInvariant() switch
        {
            "GAZ" or "GAZOLE" => TypeCarburantEnum.Diesel,
            "ESS" or "ESSENCE" => TypeCarburantEnum.Essence,
            _ => TypeCarburantEnum.Diesel
        };
    }

    private static string NormalizeImmatriculation(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var parts = value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 3)
        {
            var prefix = new string(parts[0].Where(char.IsDigit).ToArray());
            var suffix = new string(parts[^1].Where(char.IsDigit).ToArray());
            if (prefix.Length > 3) prefix = prefix[..3];
            if (suffix.Length > 4) suffix = suffix[..4];
            if (prefix.Length > 0 && suffix.Length > 0)
                return $"{prefix} TUN {suffix}";
        }

        var digits = new string(value.Where(char.IsDigit).ToArray());
        if (digits.Length >= 2)
        {
            var splitAt = Math.Clamp(digits.Length - 1, 1, 3);
            if (splitAt >= digits.Length)
                splitAt = digits.Length - 1;
            var prefix = digits[..splitAt];
            var suffix = digits[splitAt..];
            if (prefix.Length is >= 1 and <= 3 && suffix.Length is >= 1 and <= 4)
                return $"{prefix} TUN {suffix}";
        }

        return value.Trim().ToUpperInvariant();
    }
}
