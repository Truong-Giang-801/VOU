using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vou.Services.AuthAPI.Data;
using Vou.Services.AuthAPI.Models;
using Vou.Services.AuthAPI.Service.IService;

public class UserBrandService : IUserBrandService
{
    private readonly AppDbContext _context;

    public UserBrandService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserBrand>> GetUserBrandsAsync()
    {
        return await _context.UserBrand.ToListAsync();
    }

    public async Task<UserBrand?> GetUserBrandByIdAsync(int brandId, string userId)
    {
        return await _context.UserBrand
            .FirstOrDefaultAsync(ub => ub.BrandId == brandId && ub.UserID == userId);
    }

    public async Task<List<UserBrand>> GetUsersByBrandIdAsync(int brandId)
    {
        return await _context.UserBrand
            .Where(ub => ub.BrandId == brandId)
            .ToListAsync();
    }

}
