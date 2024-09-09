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
    public async Task<int?> GetBrandIdByUserIdAsync(string userId)
    {
        try
        {
            // Query the UserBrand table to find the BrandId associated with the given userId
            var brandId = await _context.UserBrand
                .Where(ub => ub.UserID == userId)
                .Select(ub => ub.BrandId)
                .FirstOrDefaultAsync();

            return brandId;
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            Console.WriteLine($"Error retrieving BrandId by userId: {ex.Message}");

            // Optionally throw or handle the exception as needed
            throw;
        }
    }

    public async Task<List<UserBrand>> GetUsersByBrandIdAsync(int brandId)
    {
        return await _context.UserBrand
            .Where(ub => ub.BrandId == brandId)
            .ToListAsync();
    }

}
