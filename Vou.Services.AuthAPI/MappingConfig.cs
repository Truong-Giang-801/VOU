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
                config.CreateMap<CreateUserDto, UserDto>();
                config.CreateMap<UserDto, CreateUserDto>();
				config.CreateMap<AssignRoleRequestDto, UserDto>();
                config.CreateMap<UserDto, AssignRoleRequestDto>();




            });
			return mappingConfig;
		}
	}
}
