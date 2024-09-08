namespace Vou.Services.AuthAPI.Models.Dto
{
    public class ActivateDeactivateUserDto
    {
        public string Identifier { get; set; }
        public bool IsActive { get; set; }
    }
}
