using AutoMapper;
using Vou.Services.EventAPI.Models;
using Vou.Services.EventAPI.Models.Dto;

namespace Vou.Services.EventAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMap()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Event, EventDto>();
				config.CreateMap<EventDto, Event>();

			});
			return mappingConfig;
		}
	}
}
