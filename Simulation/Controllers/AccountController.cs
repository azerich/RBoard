using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NETCore.MailKit.Core;
using Simulation.Data.Entities.System;
using Simulation.Data.Enums;
using Simulation.Data.System;
using Simulation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AccountController(
            UserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IEmailService emailService
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }

        [AllowAnonymous]
        public IActionResult Index() => View(nameof(AccountController.AccessDenied));

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Warning;
            TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
            TempData[nameof(ModalMessageElements.ModalBody)] = "Your are not allowed for this operation!";

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
                            TempData[nameof(ToastMessageElements.ToastMessageTitle)] = "Welcome back!";
                            TempData[nameof(ToastMessageElements.ToastMessageBody)] = lastLogin == null ? "Congratulations! This is your first authorized visit to site!" : "Last time you were on the site at " + lastLogin.ToString() + " UTC";
                            string[] url = returnUrl.ToString().Split('/');
                            if (url[1].Length > 0) return RedirectToAction(url[1], url[0]);
                            else return RedirectToAction("", url[0]);
                        }
                        else
                        {
                            TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                            TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                            TempData[nameof(ModalMessageElements.ModalBody)] = "Wrong password!";
                            return View();
                        }
                    }
                    else
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                        TempData[nameof(ModalMessageElements.ModalBody)] = "Your email is not confirmed! <br />";
                        TempData[nameof(ModalMessageElements.ModalBody)] += "You must confirm it before login!";
                        return View();
                    }
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                    TempData[nameof(ModalMessageElements.ModalBody)] = "User not found!";
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
            await signInManager.SignOutAsync().ConfigureAwait(false);
            TempData[nameof(ToastMessageElements.ToastMessageType)] = NoticeType.Succes;
            TempData[nameof(ToastMessageElements.ToastMessageIcon)] = "fa-info-circle";
            TempData[nameof(ToastMessageElements.ToastMessageMuted)] = DateTime.UtcNow.ToString("HH:mm");
            TempData[nameof(ToastMessageElements.ToastMessageTitle)] = "Bye!";
            TempData[nameof(ToastMessageElements.ToastMessageBody)] = "See you again!";
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
            return password.Length >= 8;
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
            if(await IsRegisteredUserName(model.UserName).ConfigureAwait(false)) { errorMessages.Add($"User name <b>{model.UserName}</b> is already taken."); }
            if(await IsRegisteredEmail(model.Email).ConfigureAwait(false)) { errorMessages.Add($"Email <b>{model.Email}</b> is already taken.");}
            if(!IsPasswordLengthValid(model.Password)) { errorMessages.Add("Password must be <b>8 characters</b> or more."); }
            if(!IsPasswordContainUpperLetters(model.Password)) { errorMessages.Add("Password must be contain at least one <b>upper letter</b>(e.g. A-Z)."); }
            if(!IsPasswordContainDigit(model.Password)) { errorMessages.Add("Password must be contain at least <b>one digit</b>(e.g. 0-9)."); }
            if(!IsPasswordContainSpecial(model.Password)) { errorMessages.Add("Password must be contain at least <b>one special character</b>(e.g. !@#%&*()_+=;:?.,<>№)."); }
            if(model.Password != model.ConfirmedPassword) { errorMessages.Add("Password and confirmed password do <b>not match</b>."); }

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
                    TempData[nameof(ModalMessageElements.ModalType)] = "success";
                    TempData[nameof(ModalMessageElements.ModalTitle)] = "Success!";
                    TempData[nameof(ModalMessageElements.ModalBody)] = "We send to your email verification code in link. Please, follow this link to verify email.";
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                    TempData[nameof(ModalMessageElements.ModalBody)] = "Can`t create a user with this credentials. Try another credentials!";
                    return View(model);
                }
            }
            else
            {
                TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
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
                    TempData[nameof(ToastMessageElements.ToastMessageTitle)] = "Congratulations!";
                    TempData[nameof(ToastMessageElements.ToastMessageBody)] = "Your email is confirmed. Now you can use it to sign in to site!";
                    return RedirectToAction(nameof(AccountController.Login), nameof(AccountController).CutController());
                }
                else
                {
                    TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                    TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                    TempData[nameof(ModalMessageElements.ModalBody)] = "Wrong email verification code. Try to,follow link from letter again!";
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
            }
            else
            {
                TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                TempData[nameof(ModalMessageElements.ModalBody)] = "Wrong user credentials to verify your email. Try to follow to link from letter again!";
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
                    SiteUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if(user == null)
                    {
                        TempData[nameof(ModalMessageElements.ModalType)] = NoticeType.Danger;
                        TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                        TempData[nameof(ModalMessageElements.ModalBody)] = "Something happened on site. Try again later!";
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
                        TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                        TempData[nameof(ModalMessageElements.ModalBody)] = "User not found!";
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
                        TempData[nameof(ModalMessageElements.ModalTitle)] = "Warning!";
                        TempData[nameof(ModalMessageElements.ModalBody)] = "User not found!";
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
            TempData[nameof(ToastMessageElements.ToastMessageTitle)] = "Information!";
            TempData[nameof(ToastMessageElements.ToastMessageBody)] = "Your account has been deleted from site!";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}