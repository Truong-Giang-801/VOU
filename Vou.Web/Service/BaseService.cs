using Vou.Web.Models;
using Vou.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;
using Vou.Web.Models;
using Vou.Web.Service.IService;
using static Vou.Web.Ultility.SD;
using Vou.Web.Models.Dto;

namespace Vou.Web.Service
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ITokenProvider _tokenProvider;
		public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
		{
			_httpClientFactory = httpClientFactory;
			_tokenProvider = tokenProvider;
		}
		public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
		{
			try
			{
				HttpClient client = _httpClientFactory.CreateClient("VouAPI");
				HttpRequestMessage message = new();
				message.Headers.Add("Accept", "application/json");
				//token
				if (withBearer) {
					var token = _tokenProvider.GetToken();
					message.Headers.Add("Authorization", $"Bearer {token}");
				}
				message.RequestUri = new Uri(requestDto.Url);
				if (requestDto.Data != null)
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
				}
				HttpResponseMessage? apiRespone = null;

				switch (requestDto.ApiType)
				{
					case ApiType.POST:
						message.Method = HttpMethod.Post;
						break;
					case ApiType.DELETE:
						message.Method = HttpMethod.Delete;
						break;
					case ApiType.PUT:
						message.Method = HttpMethod.Put;
						break;
					default:
						message.Method = HttpMethod.Get;
						break;
				}

				apiRespone = await client.SendAsync(message);

				switch (apiRespone.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new() { IsSuccess = false, Message = "Not Found" };
					case HttpStatusCode.Forbidden:
						return new() { IsSuccess = false, Message = "Access Denied" };
					case HttpStatusCode.Unauthorized:
						return new() { IsSuccess = false, Message = "Unauthorized" };
					case HttpStatusCode.InternalServerError:
						return new() { IsSuccess = false, Message = "InternalServerError" };
					default:
						var apiContent = await apiRespone.Content.ReadAsStringAsync();
						var apiResponeDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
						return apiResponeDto;
				}
			}
			catch (Exception ex)
			{
				var dto = new ResponseDto
				{
					IsSuccess = false,
					Message = ex.Message,
				};
				return dto;
			}
		}
	}
}