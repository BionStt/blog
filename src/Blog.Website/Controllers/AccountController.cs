using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Website.Core.Helpers;
using Blog.Website.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Blog.Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly List<LoginRestriction> _loginRestriction;

        public AccountController(ILoggerFactory loggerFactory,
                                 List<LoginRestriction> loginRestriction)
        {
            _loginRestriction = loginRestriction;
            _logger = loggerFactory.GetLogger();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction("Index", "BlogStory");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(String provider,
                                           String returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});
            var properties = new AuthenticationProperties {RedirectUri = redirectUrl};
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(String returnUrl = null,
                                                               String remoteError = null)
        {
            if(remoteError != null)
            {
                ModelState.AddModelError(String.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }

            var loginApproved = _loginRestriction.Any(x => x.Login.Equals(HttpContext.User.Claims.FirstOrDefault(p => p.Type == x.PrincipalType)?.Value));
            if(loginApproved)
            {
                var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if(authenticateResult == null ||
                   !authenticateResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                return RedirectToLocal(returnUrl);
            }

            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "BlogStory");
        }
    }
}