
using ParckingAuto.Enums;

namespace ParckingAuto.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int VehiculeId { get; set; }
        public Vehicule? Vehicule { get; set; }

        public TypeDocumentEnum TypeDocument { get; set; }   // Enum
        public string Fichier { get; set; } = string.Empty;
        public DateTime DateUpload { get; set; }
    }
}
