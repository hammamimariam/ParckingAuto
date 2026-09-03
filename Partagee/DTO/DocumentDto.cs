#nullable enable
using System;

namespace ParckingAuto.DTO
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? UploadedById { get; set; }
        public string? UploadedByName { get; set; }
    }
}
