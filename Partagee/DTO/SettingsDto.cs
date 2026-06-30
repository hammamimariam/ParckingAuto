namespace ParckingAuto.DTO
{
    public class SettingsDto
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool NotificationsEnabled { get; set; }
        public string Preferences { get; set; } = string.Empty;
    }
}