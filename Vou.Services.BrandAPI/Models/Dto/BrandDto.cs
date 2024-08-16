namespace Vou.Services.BrandAPI.Models.Dto
{
	public class BrandDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string GPS { get; set; } = string.Empty;
		public string Industry { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public DateTime DateUpdated { get; set; }
	}
}
