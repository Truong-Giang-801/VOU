using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vou.Services.AuthAPI.Models;

namespace Vou.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserBrand> UserBrand { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the UserBrand entity
            modelBuilder.Entity<UserBrand>()
                .HasKey(ub => new { ub.BrandId, ub.UserID }); // Composite key

            // Example: Configure relationships if necessary
            modelBuilder.Entity<UserBrand>()
                .HasOne<ApplicationUser>() // Adjust according to your model
                .WithMany() // Adjust according to your model
                .HasForeignKey(ub => ub.UserID);
        }
    }
}