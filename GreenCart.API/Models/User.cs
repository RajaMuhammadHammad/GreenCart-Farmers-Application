namespace GreenCart.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Automatically set to current UTC time
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow; // Automatically set to current UTC time or null
    }
}
