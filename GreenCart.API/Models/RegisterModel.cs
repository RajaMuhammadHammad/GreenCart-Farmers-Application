namespace GreenCart.API.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }  // Add Email field
    }

}
