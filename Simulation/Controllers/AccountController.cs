using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Simulation.Data.Entities.System;
using Simulation.Data.System;
using Simulation.Models;
using System;
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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync().ConfigureAwait(false);
            TempData["SuccessMessage"] = "See you again!";
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }
    }
}
