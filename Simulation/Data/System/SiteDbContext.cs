using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simulation.Data.Entities.System;
using System;

namespace Simulation.Data.System
{
    public class SiteDbContext : IdentityDbContext<SiteUser>
    {
        public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SiteUser>().HasData(new SiteUser
            {
                Id = "A34B367E-7677-4730-BAD0-13A419B0796A",
                UserName = "admin@rboard.com",
                NormalizedUserName = "ADMIN@RBOARD.COM",
                Email = "admin@rboard.com",
                NormalizedEmail = "ADMIN@RBOARD.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<SiteUser>().HashPassword(null, "superpassword"),
                RegisterDate = DateTime.UtcNow,
                ConfirmDate = DateTime.UtcNow,
                FirstName = "Site",
                LastName = "Administrator"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "3B1C34F1-C8E6-4013-AB5F-DF156968DAAE",
                Name = "Administrator"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849",
                Name = "Confirmed"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "F090C70C-FFD2-49D2-9C57-A81DF9384206",
                Name = "Registered"
            });
        }
    }
}
