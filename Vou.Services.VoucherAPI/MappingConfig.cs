using AutoMapper;
using Vou.Services.VoucherAPI.Models;
using Vou.Services.VoucherAPI.Models.Dto;

namespace Vou.Services.VoucherAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMap()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<Voucher, VoucherDto>();
				config.CreateMap<VoucherDto, Voucher>();

			});
			return mappingConfig;
		}
	}
}
