using Microsoft.AspNetCore.Identity;
using Simulation.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Data.Entities.System
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderType? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Locale { get; set; } = "en-us";
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }
}
