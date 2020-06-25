using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Simulation.Data.Entities.System;
using Simulation.Data.Enums.Site;
using Simulation.Data.Enums.Users;
using Simulation.Data.Repository;
using Simulation.Data.System;
using Simulation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simulation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<SiteUser> userManager;
        private readonly SignInManager<SiteUser> signInManager;
        private readonly IEmailService emailService;
        private readonly DataManager dataManager;
        public AccountController(
            UserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IEmailService emailService,
            DataManager dataManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.dataManager = dataManager;
        }

        [AllowAnonymous]
        public IActionResult Index() => View(nameof(AccountController.AccessDenied));

        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            LocaleType locale;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                SiteUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);
                locale = user != null ? user.Locale : SiteConfiguration.Locale;
            }
            else
            {
                locale = SiteConfiguration.Locale;
            }

            TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Warning;
            TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(locale, MessageEventType.Warning).ConfigureAwait(false);
            TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(locale, MessageEventType.YouAreNotAllowedToThisAction).ConfigureAwait(false);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, System.Uri returnUrl)
        {
            if (ModelState.IsValid)
            {
                SiteUser user = await userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (user != null)
                {
                    await signInManager.SignOutAsync().ConfigureAwait(false);
                    bool isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
                    if (isEmailConfirmed)
                    {
                        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).ConfigureAwait(false);
                        if (result.Succeeded)
                        {
                            DateTime? lastLogin = user.LastLoginDate;
                            user.LastLoginDate = DateTime.UtcNow;
                            await userManager.UpdateAsync(user).ConfigureAwait(false);
                            TempData[nameof(ToastMessageElements.ToastMessageType)] = NoticeType.Info;
                            TempData[nameof(ToastMessageElements.ToastMessageIcon)] = "fa-info-circle";
                            TempData[nameof(ToastMessageElements.ToastMessageMuted)] = DateTime.UtcNow.ToString("HH:mm");
                            TempData[nameof(ToastMessageElements.ToastMessageTitle)] =
                                lastLogin == null ?
                                    await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.Welcome).ConfigureAwait(false)
                                  : await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.WelcomeBack).ConfigureAwait(false);
                            TempData[nameof(ToastMessageElements.ToastMessageBody)] =
                                lastLogin == null ?
                                    await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.ThiIsYourFirstAuthorizedVisitToSite).ConfigureAwait(false)
                                  : await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.LastTimeYouWereOnTheSiteAt, lastLogin.ToString()).ConfigureAwait(false);
                            string[] url = returnUrl.ToString().Split('/');
                            return url[1].Length > 0 ? RedirectToAction(url[1], url[0]) : RedirectToAction("", url[0]);
                        }
                        else
                        {
                            TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                            TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.Warning).ConfigureAwait(false);
                            TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.WrongPassword).ConfigureAwait(false);
                            return View();
                        }
                    }
                    else
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.Warning).ConfigureAwait(false);
                        TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.YourEmailIsNotConfirmed, "<br/>").ConfigureAwait(false);
                        TempData[nameof(ModalMessageElements.ModalBody)] += await dataManager.LocalizedMessages.GetLocalizedMessage(user.Locale, MessageEventType.YouMustConfirmItBeforeLogin).ConfigureAwait(false);
                        return View();
                    }
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.Warning).ConfigureAwait(false);
                    TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.UserNotFound).ConfigureAwait(false);
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            SiteUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);
            LocaleType locale = user != null ? user.Locale : SiteConfiguration.Locale;
            TempData[nameof(ToastMessageElements.ToastMessageType)] = NoticeType.Succes;
            TempData[nameof(ToastMessageElements.ToastMessageIcon)] = "fa-info-circle";
            TempData[nameof(ToastMessageElements.ToastMessageMuted)] = DateTime.UtcNow.ToString("HH:mm");
            TempData[nameof(ToastMessageElements.ToastMessageTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(locale, MessageEventType.Bye).ConfigureAwait(false);
            TempData[nameof(ToastMessageElements.ToastMessageBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(locale, MessageEventType.SeeYouAgain).ConfigureAwait(false);
            await signInManager.SignOutAsync().ConfigureAwait(false);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
                await signInManager.SignOutAsync().ConfigureAwait(false);
            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> IsRegisteredUserName(string username)
        {
            SiteUser user = await userManager.FindByNameAsync(username).ConfigureAwait(false);
            return user == null;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> IsRegisteredEmail(string email)
        {
            SiteUser user = await userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user != null;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordLengthValid(string password)
        {
            return password.Length >= SiteConfiguration.PasswordMinLength;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainUpperLetters(string password)
        {
            Regex regex = new Regex("[A-Z]");
            MatchCollection matches = regex.Matches(password);
            return matches.Count > 0;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainDigit(string password)
        {
            Regex regex = new Regex("[0-9]");
            MatchCollection matches = regex.Matches(password);
            return matches.Count > 0;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainSpecial(string password)
        {
            Regex regex = new Regex("[!@#%&*()_+=;:?.,<>№]");
            MatchCollection matches = regex.Matches(password);
            return matches.Count > 0;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            bool isValid = false;
            List<string> errorMessages = new List<string>();
            if (await IsRegisteredUserName(model.UserName).ConfigureAwait(false))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.UserName, $"<b>{model.UserName}").ConfigureAwait(false);
                message += " " +await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.IsAlreadyTaken).ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(await IsRegisteredEmail(model.Email).ConfigureAwait(false))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.Email, $"<b>{model.Email}</b>").ConfigureAwait(false);
                message += " " + await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.IsAlreadyTaken).ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(!IsPasswordLengthValid(model.Password))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.PasswordMustBeContainAtLeast, $"<b>{SiteConfiguration.PasswordMinLength}</b>").ConfigureAwait(false);
                message += await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.OrMore).ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(!IsPasswordContainUpperLetters(model.Password))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.PasswordMustBeContainAtLeast, "<b>one upper letter</b>").ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(!IsPasswordContainDigit(model.Password))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.PasswordMustBeContainAtLeast, "<b>one digit</b>").ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(!IsPasswordContainSpecial(model.Password))
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.PasswordMustBeContainAtLeast, "<b>one special character</b>").ConfigureAwait(false);
                errorMessages.Add(message);
            }
            if(model.Password != model.ConfirmedPassword)
            {
                string message = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.PasswordMismatch).ConfigureAwait(false);
                errorMessages.Add(message);
            }

            isValid = (errorMessages.Count == 0);

            if (isValid)
            {
                SiteUser user = new SiteUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = new PasswordHasher<SiteUser>().HashPassword(null, model.Password)
                };

                IdentityResult registerResult = await userManager.CreateAsync(user).ConfigureAwait(false);
                if(registerResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Registered").ConfigureAwait(false);
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    string link = Url.Action(
                        nameof(AccountController.VerifyEmail),
                        nameof(AccountController).CutController(),
                        new {userid = user.Id, code},
                        Request.Scheme,
                        Request.Host.ToString()
                        );
                    await emailService.SendAsync(user.Email, "[RBoard] Email verification", $"Follow <a href=\"{link}\" target=\"_blank\">this link</a> to verify your email on site.", true).ConfigureAwait(false);
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Succes;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeSuccess).ConfigureAwait(false);
                    TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.WeSendToYourEmailVerificationCode).ConfigureAwait(false);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                    TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.CanNotCreateAUser).ConfigureAwait(false);
                    return View(model);
                }
            }
            else
            {
                TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                TempData[nameof(ModalMessageElements.ModalBody)] = "<ul>";
                foreach (string error in errorMessages)
                {
                    TempData[nameof(ModalMessageElements.ModalBody)] += $"<li>{error}</li>";
                }
                TempData[nameof(ModalMessageElements.ModalBody)] += "</ul>";
                return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string userid, string code)
        {
            SiteUser user = await userManager.FindByIdAsync(userid).ConfigureAwait(false);
            if (user != null)
            {
                IdentityResult confirmResult = await userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
                if (confirmResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Confirmed").ConfigureAwait(false);
                    TempData[nameof(ToastMessageElements.ToastMessageType)] = NoticeType.Succes;
                    TempData[nameof(ToastMessageElements.ToastMessageIcon)] = "fa-info-circle";
                    TempData[nameof(ToastMessageElements.ToastMessageMuted)] = DateTime.UtcNow.ToString("HH:mm");
                    TempData[nameof(ToastMessageElements.ToastMessageTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeSuccess).ConfigureAwait(false);
                    TempData[nameof(ToastMessageElements.ToastMessageBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.YourEmailIsConfirmed).ConfigureAwait(false);
                    return RedirectToAction(nameof(AccountController.Login), nameof(AccountController).CutController());
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                    TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.WrongEmailVerificationCode).ConfigureAwait(false);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
            }
            else
            {
                TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.WrongUserCredentialsToVerifyYourEmail).ConfigureAwait(false);
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
        }

        public async Task<IActionResult> ProfileView(string username)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                SiteUserViewModel model;
                if (string.IsNullOrEmpty(username))
                {
                    SiteUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name).ConfigureAwait(false);
                    if(user == null)
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                        TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.SiteError).ConfigureAwait(false);
                        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                    }
                    else
                    {
                        model = new SiteUserViewModel(user);
                        return View(model);
                    }
                }
                else
                {
                    SiteUser user = await userManager.FindByNameAsync(username).ConfigureAwait(false);
                    if (user != null)
                    {
                        model = new SiteUserViewModel(user);
                        return View(model);
                    }
                    else
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Warning;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                        TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.UserNotFound).ConfigureAwait(false);
                        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction(nameof(AccountController.AccessDenied), nameof(HomeController).CutController());
                }
                else
                {
                    SiteUser user = await userManager.FindByNameAsync(username).ConfigureAwait(false);
                    if (user != null)
                    {
                        SiteUserViewModel model = new SiteUserViewModel(user);
                        return View(model);
                    }
                    else
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Warning;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeWarning).ConfigureAwait(false);
                        TempData[nameof(ModalMessageElements.ModalBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.UserNotFound).ConfigureAwait(false);
                        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                    }
                }
            }
        }

        public async Task<IActionResult> DeleteAccount()
        {
            await signInManager.SignOutAsync().ConfigureAwait(false);

            SiteUser user = await userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            IList<string> roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            await userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);
            await userManager.DeleteAsync(user).ConfigureAwait(false);

            TempData[nameof(ToastMessageElements.ToastMessageType)] = NoticeType.Info;
            TempData[nameof(ToastMessageElements.ToastMessageIcon)] = "fa-info-circle";
            TempData[nameof(ToastMessageElements.ToastMessageMuted)] = DateTime.UtcNow.ToString("HH:mm");
            TempData[nameof(ToastMessageElements.ToastMessageTitle)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.NoticeTypeInfo).ConfigureAwait(false);
            TempData[nameof(ToastMessageElements.ToastMessageBody)] = await dataManager.LocalizedMessages.GetLocalizedMessage(SiteConfiguration.Locale, MessageEventType.AccountDeleted).ConfigureAwait(false);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}