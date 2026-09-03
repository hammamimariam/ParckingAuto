namespace ParckingAuto.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public int? UtilisateurId { get; set; }  // Who did it
        public Utilisateur? Utilisateur { get; set; }
        public string ActionType { get; set; } = string.Empty;  // Login, Logout, Create, Update, Delete
        public string TableName { get; set; } = string.Empty;  // Which table was affected
        public string? RecordId { get; set; }  // ID of the affected record
        public string? OldValues { get; set; }  // JSON of old values (for updates/deletes)
        public string? NewValues { get; set; }  // JSON of new values (for creates/updates)
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
