using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Vou.Services.BrandAPI.Models;

namespace Vou.Services.BrandAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Brand> Brand { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Brand>().HasData(new Brand
			{
				Id = 1,
				Name = "Brand 1",
				GPS = "123123",
				Industry = "Cocacola",

			});
			modelBuilder.Entity<Brand>().HasData(new Brand
			{
				Id = 2,
				Name = "Brand 2",
				GPS = "111,111",
				Industry = "Cocacola",

			});
		}
	}
}
