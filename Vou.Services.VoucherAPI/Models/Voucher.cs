using System.ComponentModel.DataAnnotations;

namespace Vou.Services.VoucherAPI.Models
{
	public class Voucher
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Code{ get; set; } = string.Empty;
		[Required]
		public string QRCode { get; set; } = string.Empty;
		
		public string Img { get; set; } = string.Empty ;

		public int Value { get; set; }

		public string Description { get; set; } = string.Empty;	

		public bool State {  get; set; } = false;
		public DateTime ExpireDate { get; set; } 

	}
}
