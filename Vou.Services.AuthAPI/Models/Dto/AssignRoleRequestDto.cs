namespace Vou.Services.AuthAPI.Models.Dto
{
    public class AssignRoleRequestDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
