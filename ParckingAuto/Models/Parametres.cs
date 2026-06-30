namespace ParckingAuto.Models
{
    public class Parametres
    {
        public int Id { get; set; }
        public bool NotifVidange { get; set; } = true;
        public bool NotifAssurance { get; set; } = true;
        public bool NotifVisiteTech { get; set; } = true;
        public bool NotifPermis { get; set; } = true;
    }
}
