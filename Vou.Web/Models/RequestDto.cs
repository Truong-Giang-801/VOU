using System.Net.Mime;
using System.Security.AccessControl;

using static Vou.Web.Ultility.SD;

namespace Vou.Web.Models
{
	public class RequestDto
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public string Url { get; set; }
		public object Data { get; set; }
		public string AccessToken { get; set; }

	}
}
