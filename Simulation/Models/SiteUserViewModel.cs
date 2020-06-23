using Microsoft.AspNetCore.Identity;
using Simulation.Data.Entities.System;
using Simulation.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Simulation.Models
{
    public class SiteUserViewModel
    {
        public SiteUserViewModel(SiteUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Gender = user.Gender;
            BirthDate = user.BirthDate;
            Locale = user.Locale;
            RegisterDate = user.RegisterDate.ToString();
            ConfirmDate = user.ConfirmDate.ToString();
            LastLoginDate = user.LastLoginDate.ToString();
        }
        [Display(Name ="Id")]
        [Required]
        public string Id { get; set; }

        [Display(Name = "User name")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "First name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Middle name")]
        [Required]
        public string MiddleName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        [UIHint("radio")]
        [Required]
        public GenderType? Gender { get; set; }

        [Display(Name ="Birth date")]
        [UIHint("date")]
        public DateTime? BirthDate { get; set; }
        
        [Display(Name = "Language")]
        public string Locale { get; set; } = "en-us";
        
        [Display(Name = "Register date")]
        public string RegisterDate { get; set; } = DateTime.UtcNow.ToString();
        
        [Display(Name = "Confirm date")]
        public string ConfirmDate { get; set; }

        [Display(Name = "Last login date")]
        public string LastLoginDate { get; set; }
    }
}
