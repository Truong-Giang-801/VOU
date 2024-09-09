using System.ComponentModel.DataAnnotations;

namespace Vou.Services.EventAPI.Models
{
	public class Event
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int BrandId	{ get; set; }
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Img { get; set; } = string.Empty;
		
		public int NumberOfVoucher { get; set; }
		public DateTime DateCreated { get; set; } 
		public DateTime DateUpdated { get; set; } 
	}
}
