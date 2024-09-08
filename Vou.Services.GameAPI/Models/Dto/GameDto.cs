using System.ComponentModel.DataAnnotations;

namespace Vou.Services.GameAPI.Models.Dto
{
	public class GameDto
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;

        public bool AllowTrade { get; set; } = false;
        public string Instruction { get; set; } = string.Empty;
    }
}
