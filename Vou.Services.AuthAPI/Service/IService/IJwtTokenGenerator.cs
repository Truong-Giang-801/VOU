using Vou.Services.AuthAPI.Models;

namespace Vou.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles );
    }
}
