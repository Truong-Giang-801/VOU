using Vou.Web.Models;
using Vou.Web.Models.Dto;
using Vou.Web.Service.IService;
using Vou.Web.Ultility;

namespace Vou.Web.Service
{
	public class BrandService : IBrandService
	{
		private readonly IBaseService _baseService;

		public BrandService(IBaseService baseService)
		{
			_baseService = baseService;
		}


		public async Task<ResponseDto?> CreateBrandAsync(BrandDto brandDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Ultility.SD.ApiType.POST,
				Data = brandDto,
				Url = SD.BrandAPIBase + "/api/brand"
			});
		}

		public async Task<ResponseDto?> DeleteBrandAsync(int id)
		{
            //return await _baseService.SendAsync(new RequestDto()
            //{
            //	ApiType = Ultility.SD.ApiType.DELETE,
            //	Url = SD.BrandAPIBase + "/api/brand/"+id
            //});
            var request = new RequestDto()
            {
                ApiType = Ultility.SD.ApiType.DELETE,
                Url = SD.BrandAPIBase + "/api/brand/" + id
            };

            var response = await _baseService.SendAsync(request);

            // Log the raw response
            Console.WriteLine("Raw API response: " + response?.Result);

            return response;
        }

		public async Task<ResponseDto?> GetAllBrandAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Ultility.SD.ApiType.GET,
				Url = SD.BrandAPIBase + "/api/brand"
			});
		}

		public async Task<ResponseDto?> GetBrandByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Ultility.SD.ApiType.GET,
				Url = SD.BrandAPIBase + "/api/brand/" + id
			});
		}

		public async Task<ResponseDto?> UpdateBrandAsync(BrandDto brandDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = Ultility.SD.ApiType.PUT,
				Data = brandDto,
				Url = SD.BrandAPIBase + "/api/brand/" + brandDto.Id
			});
		}
	}
}
