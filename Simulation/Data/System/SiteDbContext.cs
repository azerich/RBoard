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

        public DbSet<LocalizedMessage> LocalizedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SiteUser>().HasData(new SiteUser
            {
                Id = "A34B367E-7677-4730-BAD0-13A419B0796A",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
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
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849",
                Name = "Confirmed",
                NormalizedName = "CONFIRMED"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "F090C70C-FFD2-49D2-9C57-A81DF9384206",
                Name = "Registered",
                NormalizedName = "REGISTERED"
            });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "F090C70C-FFD2-49D2-9C57-A81DF9384206",
                UserId = "A34B367E-7677-4730-BAD0-13A419B0796A"
            }, new IdentityUserRole<string>
            {
                RoleId = "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849",
                UserId = "A34B367E-7677-4730-BAD0-13A419B0796A"
            }, new IdentityUserRole<string>
            {
                RoleId = "3B1C34F1-C8E6-4013-AB5F-DF156968DAAE",
                UserId = "A34B367E-7677-4730-BAD0-13A419B0796A"
            });
        }
    }
}
