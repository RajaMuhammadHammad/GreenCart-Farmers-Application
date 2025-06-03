using System.ComponentModel.DataAnnotations;

namespace GreenCartFarmers.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

}
