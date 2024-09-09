using System.Threading.Tasks;
using Vou.Services.AuthAPI.Models;

namespace Vou.Services.AuthAPI.Service.IService
{
    public interface IUserBrandService
    {
        Task<UserBrand> GetUserBrandByIdAsync(int brandId, string userId);
        Task<List<UserBrand>> GetUserBrandsAsync();
        Task<int?> GetBrandIdByUserIdAsync(string userId);
        Task<List<UserBrand>> GetUsersByBrandIdAsync(int brandId); // get all user by brand id
    }
}
