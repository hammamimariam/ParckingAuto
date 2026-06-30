namespace ParckingAuto.Models
{
    public class Mouvement
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule? Vehicule { get; set; }

        public int ChauffeurId { get; set; }
        public Chauffeur? Chauffeur { get; set; }

        public DateTime DateDepart { get; set; }
        public DateTime? DateRetour { get; set; }
        public int KmDepart { get; set; }
        public int? KmRetour { get; set; }
        public string Destination { get; set; } = string.Empty;
    }
}
