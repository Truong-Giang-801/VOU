namespace Vou.Services.VoucherAPI.Models.Dto
{
	public class ResponeDto
	{
		public object? Result { get; set; }
		public bool? IsSuccess { get; set; } = true;
		public string Message { get; set; } = string.Empty;
	}
}
