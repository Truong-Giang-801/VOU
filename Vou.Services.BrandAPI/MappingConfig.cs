using AutoMapper;
using Vou.Services.BrandAPI.Models;
using Vou.Services.BrandAPI.Models.Dto;

namespace Vou.Services.BrandAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMap()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Brand, BrandDto>();
				config.CreateMap<BrandDto, Brand>();

			});
			return mappingConfig;
		}
	}
}
