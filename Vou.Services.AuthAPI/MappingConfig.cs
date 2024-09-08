using AutoMapper;
using Vou.Services.AuthAPI.Models;
using Vou.Services.AuthAPI.Models.Dto;


namespace Vou.Services.AuthAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMap()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<ApplicationUser, UserDto>();
				config.CreateMap<UserDto, ApplicationUser>();
				config.CreateMap<UserBrand, UserBrandDto>();
				config.CreateMap<UserBrandDto, UserBrand>();

			});
			return mappingConfig;
		}
	}
}
