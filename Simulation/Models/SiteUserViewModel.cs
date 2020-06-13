using Simulation.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Simulation.Models
{
    public class SiteUserViewModel
    {
        [Display(Name ="Your Id on site")]
        public string Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public GenderType? Gender { get; set; }

        [Display(Name ="Birth date")]
        [UIHint("date")]
        public DateTime? BirthDate { get; set; }
        
        [Display(Name = "Language")]
        public string Locale { get; set; } = "en-us";
        
        [Display(Name = "Register date")]
        [UIHint("date")]
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Confirm date")]
        [UIHint("date")]
        public DateTime? ConfirmDate { get; set; }

        [Display(Name = "Last login date")]
        [UIHint("date")]
        public DateTime? LastLoginDate { get; set; }
    }
}
