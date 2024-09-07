using Vou.Web.Models;
using Vou.Web.Models.Dto;

namespace Vou.Web.Service.IService
{
	public interface IBaseService
	{
		Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
	}
}
