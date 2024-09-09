using System.Diagnostics.Eventing.Reader;

namespace Vou.Services.AuthAPI.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Lockout { get; set; } =string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
