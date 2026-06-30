namespace ParckingAuto.Models
{
    public class Carburant
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule? Vehicule { get; set; }

        public DateTime DatePlein { get; set; }
        public decimal VolumeLitres { get; set; }
        public decimal Montant { get; set; }
        public int Kilometrage { get; set; }
    }
}
