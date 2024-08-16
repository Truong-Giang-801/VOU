using Vou.Web.Models.Dto;

namespace Vou.Web.Service.IService
{
	public interface IBrandService 
	{
		Task<ResponseDto?> GetBrandByIdAsync(int id);
		Task<ResponseDto?> GetAllBrandAsync();
		Task<ResponseDto?> CreateBrandAsync(BrandDto brandDto);
		Task<ResponseDto?> UpdateBrandAsync(BrandDto brandDto);
		Task<ResponseDto?> DeleteBrandAsync(int id);

	}
}
