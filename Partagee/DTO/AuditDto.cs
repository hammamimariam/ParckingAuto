#nullable enable
using System;

namespace ParckingAuto.DTO
{
    public class AuditDto
    {
        public int Id { get; set; }
        public int? UtilisateurId { get; set; }
        public string? UtilisateurNom { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string? RecordId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
