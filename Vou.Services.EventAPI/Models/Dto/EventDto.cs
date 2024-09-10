using System.ComponentModel.DataAnnotations;

namespace Vou.Services.EventAPI.Models.Dto
{
	public class EventDto
	{
        public int Id { get; set; }
        public int BrandId  { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;

        public int NumberOfVoucher { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
