using System.ComponentModel.DataAnnotations;

namespace Vou.Services.AuthAPI.Models.Dto
{
    public class LoginRequestDto
    {
        
        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
