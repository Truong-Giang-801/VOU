using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Vou.Services.EventAPI.Models;

namespace Vou.Services.EventAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Event> Event { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Event>().HasData(new Event
			{
				Id = 1,
				BrandId = 1,
				GameId = 1,
				Name = "Event 1",
				Img = "123123",
				NumberOfVoucher = 1,

			});
			modelBuilder.Entity<Event>().HasData(new Event
			{
				Id = 2,
				BrandId = 1,
				GameId= 1,
				Name = "Event 2",
                Img = "123123",
                NumberOfVoucher = 1,

			});
        }
	}
}
