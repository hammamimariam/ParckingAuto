namespace ParckingAuto.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule Vehicule { get; set; } = null!;
        public string TypeDocument { get; set; } = string.Empty;
        public string Fichier { get; set; } = string.Empty;
        public DateTime DateUpload { get; set; } = DateTime.Now;
    }
}
