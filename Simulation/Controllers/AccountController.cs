using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Simulation.Data.Entities.System;
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
        public IActionResult Index() => RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

        [AllowAnonymous]
        public IActionResult AccessDenied() => RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());

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
                            user.LastLoginDate = DateTime.UtcNow;
                            TempData["SuccessMessage"] = "Welcome back!";
                            if(returnUrl != null)
                            {
                                string[] url = returnUrl.ToString().Split('/');
                                if(url[1].Length != 0)
                                {
                                    return RedirectToAction(url[1], url[0]);
                                }
                                else
                                {
                                    return RedirectToAction("", url[0]);
                                }
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                        else
                        {
                            TempData["DangerMessage"] = "Wrong password!";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["DangerMessage"] = "Your email is not confirmed! <br />";
                        TempData["DangerMessage"] += " You must confirm it before login!";
                        return View();
                    }
                }
                else
                {
                    TempData["DangerMessage"] = "User not found!";
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
            TempData["SuccessMessage"] = "See you again!";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await signInManager.SignOutAsync();
            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> IsRegisteredEmail(string email)
        {
            SiteUser user = await userManager.FindByEmailAsync(email);
            if (user != null) return true;
            else return false;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordLengthValid(string password)
        {
            if (password.Length < 8) return false;
            else return true;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainUpperLetters(string password)
        {
            Regex regex = new Regex(@"[A-Z]");
            MatchCollection matches = regex.Matches(password);
            if (matches.Count > 0) return true;
            else return false;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainDigit(string password)
        {
            Regex regex = new Regex(@"[0-9]");
            MatchCollection matches = regex.Matches(password);
            if (matches.Count > 0) return true;
            else return false;
        }

        [AllowAnonymous]
        [HttpGet]
        public bool IsPasswordContainSpecial(string password)
        {
            string[] specialChars = new[] { "&", "+", "@", "$", "#", "%", "!", "-", "_", "/" };
            int result = 0;
            for (int i = 0; i < password.Length; i++)
            {
                for (int j = 0; j < specialChars.Length; j++)
                {
                    if (password[i] == Convert.ToChar(specialChars[j])) result++;
                }
            }
            if (result > 0) return true;
            else return false;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            bool isValid = false;
            List<string> errorMessages = new List<string>();
            if(await IsRegisteredEmail(model.Email)) { errorMessages.Add($"Email <b>{model.Email}</b> is already taken.");}
            if(!IsPasswordLengthValid(model.Password)) { errorMessages.Add("Password must be <b>8 characters</b> or more."); }
            if(!IsPasswordContainUpperLetters(model.Password)) { errorMessages.Add("Password must be contain at least one <b>upper letter</b>."); }
            if(!IsPasswordContainDigit(model.Password)) { errorMessages.Add("Password must be contain at least <b>one digit</b>."); }
            if(!IsPasswordContainSpecial(model.Password)) { errorMessages.Add("Password must be contain at least <b>one special character</b>."); }
            if(model.Password != model.ConfirmedPassword) { errorMessages.Add("Password and confirmed password do <b>not match</b>."); }

            if (errorMessages.Count > 0) isValid = false;
            else isValid = true;

            if (isValid)
            {
                SiteUser user = new SiteUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = new PasswordHasher<SiteUser>().HashPassword(null, model.Password)
                };

                IdentityResult registerResult = await userManager.CreateAsync(user);
                if(registerResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Registered");
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action(
                        nameof(AccountController.VerifyEmail),
                        nameof(AccountController).CutController(),
                        new {userid = user.Id, code},
                        Request.Scheme,
                        Request.Host.ToString()
                        );
                    await emailService.SendAsync(user.Email, "[RBoard] Email verification", $"Follow <a href=\"{link}\" target=\"_blank\">this link</a> to verify your email on site.", true);
                    TempData["SuccessMessage"] = "We send to your email verification code in link. Please, follow this link to verify email.";
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
                else
                {
                    TempData["WarningMessage"] = "Can`t create a user with this credentials. Try another credentials!";
                    return View(model);
                }
            }
            else
            {
                TempData["WarningMessage"] = "<ul>";
                foreach (string error in errorMessages)
                {
                    TempData["WarningMessage"] += $"<li>{error}</li>";
                }
                TempData["WarningMessage"] += "</ul>";
                return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string userid, string code)
        {
            SiteUser user = await userManager.FindByIdAsync(userid);
            if (user != null)
            {
                IdentityResult confirmResult = await userManager.ConfirmEmailAsync(user, code);
                if (confirmResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Confirmed");
                    TempData["SuccessMessage"] = "Congratulations! Your email is confirmed. Now you can use it to sign in to site!";
                    return RedirectToAction(nameof(AccountController.Login), nameof(AccountController).CutController());
                }
                else
                {
                    TempData["WarningMessage"] = "Wrong email verification code. Try to,follow link from letter again!";
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
                }
            }
            else
            {
                TempData["WarningMessage"] = "Wrong user credentials to verify your email. Try to follow to link from letter again!";
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
            }
        }

        public async Task<IActionResult> Profile()
        {
            SiteUser user = await userManager.GetUserAsync(HttpContext.User);
            SiteUserViewModel viewModel = new SiteUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Locale = user.Locale,
                RegisterDate = user.RegisterDate,
                ConfirmDate = user.ConfirmDate,
                LastLoginDate = user.LastLoginDate
            };
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteAccount()
        {
            await signInManager.SignOutAsync();

            SiteUser user = await userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            IList<string> roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            
            await userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);
            await userManager.DeleteAsync(user).ConfigureAwait(false);

            TempData["InfoMessage"] = "Your account has been deleted from site.";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

    }
}