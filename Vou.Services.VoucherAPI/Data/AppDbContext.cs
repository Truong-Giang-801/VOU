using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Vou.Services.VoucherAPI.Models;

namespace Vou.Services.VoucherAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Voucher> Voucher { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<Voucher>().HasData(new Voucher
			{
				Id = 1,
				Code = "Voucher 1",
				Img = "123123",
				QRCode = "123213213",
				Value = 10,
                Description ="Voucher vip",
                State = true,
                ExpireDate = DateTime.Now.AddDays(7),


            });
			modelBuilder.Entity<Voucher>().HasData(new Voucher
			{
                Id = 2,
                Code = "Voucher 2",
                Img = "123123",
                QRCode = "123213213",
                Value = 10,
                Description = "Voucher vip",
                State = true,
                ExpireDate = DateTime.Now.AddDays(7),

            });
        }
	}
}
