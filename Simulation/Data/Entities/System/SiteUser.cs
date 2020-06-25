using Microsoft.AspNetCore.Identity;
using Simulation.Data.Enums.Users;
using System;

namespace Simulation.Data.Entities.System
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public LocaleType Locale { get; set; } = LocaleType.en;
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        
        public void SetLastLoginDate(DateTime last)
        {
            LastLoginDate = last;
        }
        public DateTime? GetLastLoginDate()
        {
            return LastLoginDate;
        }
    }
}
