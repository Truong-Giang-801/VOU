using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Vou.Services.GameAPI.Models;

namespace Vou.Services.GameAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Game> Game { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Game>().HasData(new Game
			{
				Id = 1,
				Name = "Game 1",
				Img = "123123",
				AllowTrade = true,
				Instruction = "just answer the question",

			});
			modelBuilder.Entity<Game>().HasData(new Game
			{
				Id = 2,
				Name = "Game 2",
                Img = "123123",
                AllowTrade = true,
				Instruction = "shake your phone",

			});
        }
	}
}
