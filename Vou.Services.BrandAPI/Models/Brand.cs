using System.ComponentModel.DataAnnotations;

namespace Vou.Services.BrandAPI.Models
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string GPS { get; set; } = string.Empty;
		[Required]
		public string Industry { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime DateUpdated { get; set; } 
	}
}
