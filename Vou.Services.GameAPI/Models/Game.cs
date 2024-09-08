using System.ComponentModel.DataAnnotations;

namespace Vou.Services.GameAPI.Models
{
	public class Game
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Img { get; set; } = string.Empty;
		
		public bool AllowTrade { get; set; } = false;
		public string Instruction { get; set; } = string.Empty ;

	}
}
