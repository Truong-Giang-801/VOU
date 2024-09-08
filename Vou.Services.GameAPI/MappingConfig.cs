using AutoMapper;
using Vou.Services.GameAPI.Models;
using Vou.Services.GameAPI.Models.Dto;

namespace Vou.Services.GameAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMap()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Game, GameDto>();
				config.CreateMap<GameDto, Game>();

			});
			return mappingConfig;
		}
	}
}
