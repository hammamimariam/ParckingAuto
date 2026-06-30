namespace ParckingAuto.DTO
{
    public class UpdateUserRequest
    {
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? MotDePasse { get; set; }
    }
}
