using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simulation.Data.Entities.System;
using Simulation.Data.Enums.Site;
using Simulation.Data.Enums.Users;
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
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>{ RoleId = "F090C70C-FFD2-49D2-9C57-A81DF9384206", UserId = "A34B367E-7677-4730-BAD0-13A419B0796A" },
                new IdentityUserRole<string>{ RoleId = "81F5E7BF-CAD7-4EEE-8D8B-2ABB2B071849", UserId = "A34B367E-7677-4730-BAD0-13A419B0796A" },
                new IdentityUserRole<string>{ RoleId = "3B1C34F1-C8E6-4013-AB5F-DF156968DAAE", UserId = "A34B367E-7677-4730-BAD0-13A419B0796A" }
            );
            //default StringWords: localization - en
            modelBuilder.Entity<LocalizedMessage>().HasData(
                new LocalizedMessage { Id = new Guid("B2EA1DAB-1924-4EA6-9E43-A0194E21855D"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Bye, Message = "Bye" },
                new LocalizedMessage { Id = new Guid("BF562D8B-B10F-47DE-95E1-1CDB8DD236D5"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Danger, Message = "Danger" },
                new LocalizedMessage { Id = new Guid("087620E1-D585-4259-98B2-87801C9AF9B2"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Dark, Message = "Dark" },
                new LocalizedMessage { Id = new Guid("E8BBB596-CADF-41D2-872C-A0D6C63002BA"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Email, Message = "Email" },
                new LocalizedMessage { Id = new Guid("672BA02A-A64F-40A8-B73F-A72C22542300"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Info, Message = "Information" },
                new LocalizedMessage { Id = new Guid("C9F5B633-7BE0-4DFD-BBA4-03D1D8EE6D66"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Light, Message = "Tip" },
                new LocalizedMessage { Id = new Guid("30505114-973A-4F4E-BD98-EC28FD757A7C"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Primary, Message = "Primary" },
                new LocalizedMessage { Id = new Guid("6C2E94A9-0FDE-4114-91EE-5D9C50251FE5"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Secondary, Message = "Secondary" },
                new LocalizedMessage { Id = new Guid("575A4A2D-C1A3-4126-BCF1-D5DC8493C7A4"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Success, Message = "Success" },
                new LocalizedMessage { Id = new Guid("05DA1568-92E2-4C13-8FB3-875B47FF7D99"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.UserName, Message = "User name" },
                new LocalizedMessage { Id = new Guid("BC26E07B-9A27-4A39-AF0D-472373015A0F"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Warning, Message = "Warning" },
                new LocalizedMessage { Id = new Guid("4F1C35EC-0BFC-44D7-B018-84353763F10E"), Locale = LocaleType.en, Sentence = StringSentences.IsNull, Word = StringWords.Welcome, Message = "Welcome" }
            ) ;
            //default StringSentences: localization - en
            modelBuilder.Entity<LocalizedMessage>().HasData(
                new LocalizedMessage { Id = new Guid("FE54E1F3-7707-4D81-9412-C0F8A847D176"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.AccountDeleted, Message = "Account deleted"},
                new LocalizedMessage { Id = new Guid("AEDBC25C-BB65-4F14-92EA-D7C91206E9E4"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.CanNotCreateAUser, Message = "Can not create a user"},
                new LocalizedMessage { Id = new Guid("8F858BAA-2846-4611-91F8-DF3D253F7A67"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.IsAlreadyTaken, Message = "is already taken"},
                new LocalizedMessage { Id = new Guid("D7290A7F-4053-4765-92BD-9FDA5D725EFC"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.LastTimeYouWereOnTheSiteAt, Message = "Last time you were on the site is"},
                new LocalizedMessage { Id = new Guid("EB1D39AB-6946-4654-A2B7-D31DA9A0B4BB"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.OrMore, Message = "or more"},
                new LocalizedMessage { Id = new Guid("D30FCD3D-C26D-4B2C-968D-08AD0888F76F"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.PasswordMustContainAtLeast, Message = "Password must contain at least"},
                new LocalizedMessage { Id = new Guid("7F7CF1A6-0BF0-4378-BAEE-6146CE977489"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.PasswordsMismatch, Message = "Passwords mismatch"},
                new LocalizedMessage { Id = new Guid("B0ABABD4-FCD0-43F9-8669-86815F95E700"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.SeeYouAgain, Message = "See you again"},
                new LocalizedMessage { Id = new Guid("EEA5C066-842B-4608-BFEC-D706E1ECE19C"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.SiteError, Message = "Site error"},
                new LocalizedMessage { Id = new Guid("C696CF4E-7A29-41FB-97AA-CF94E78773EE"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.ThiIsYourFirstAuthorizedVisitToSite, Message = "This is your first authorized visit to site"},
                new LocalizedMessage { Id = new Guid("F180937D-D98A-4660-BC82-41DCE5966B5B"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.UserNotFound, Message = "User not found"},
                new LocalizedMessage { Id = new Guid("B0E6ED47-9B54-4936-AF41-200C22BD819A"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.VerificationCodeSendedToEmail, Message = "Verification code successfully send to your email"},
                new LocalizedMessage { Id = new Guid("E526E4E8-F85E-4467-B58D-9C1C2F042315"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.WrongEmailVerificationCode, Message = "Wrong Email verification code"},
                new LocalizedMessage { Id = new Guid("AFE92478-A9E0-4422-88BD-F206E9AB5DB5"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.WrongPassword, Message = "Wrong password"},
                new LocalizedMessage { Id = new Guid("4DFDAFF6-603E-4859-BEF7-0B33D7D74BF8"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.WrongUserCredentialsToVerifyYourEmail, Message = "Wrong user credentials to verify your email"},
                new LocalizedMessage { Id = new Guid("A22AA690-5B3D-4445-8DE4-4B742A2FEDBE"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.YouAreNotAllowed, Message = "You are not allowed"},
                new LocalizedMessage { Id = new Guid("AA5B2B3F-A6B3-49C3-99D5-CCA871F6A53F"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.YouMustConfirmItBeforeLogin, Message = "You must confirm it before login"},
                new LocalizedMessage { Id = new Guid("02D3EE00-2F51-4D89-8699-9BA120C26D18"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.YourEmailIsConfirmed, Message = "Your Email is confirmed"},
                new LocalizedMessage { Id = new Guid("53704287-05C8-4A2C-8A8B-62D6F8D7A35B"), Locale = LocaleType.en, Word = StringWords.IsNull, Sentence = StringSentences.YourEmailIsNotConfirmed, Message = "Your Email is not confirmed"}
            );
        }
    }
}
