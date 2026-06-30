namespace ParckingAuto.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule? Vehicule { get; set; }

        public string TypeIntervention { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateIntervention { get; set; }
        public int KilometrageIntervention { get; set; }
        public decimal Cout { get; set; }
        public string Fournisseur { get; set; } = string.Empty;
        public string Facture { get; set; } = string.Empty;
    }
}
