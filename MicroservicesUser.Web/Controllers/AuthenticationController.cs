using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            string token = await _authenticationServices.LoginUser(loginVM);
            if (token == Messages.AuthenticationFailed)
            {
                TempData["ErrorMessage"] = "Incorrect email or password, please try again.";
                return View(loginVM);
            }
            else
            {
                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("AuthToken", token, cookieOptions);
                TempData["SuccessMessage"] = "Logged in successful.";
                return RedirectToAction("Index", "Dashboard");
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(LoginVM loginVM)
        {
            string result = await _authenticationServices.ForgotPassword(loginVM.Email);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "Email sent successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect email address!";
            }
            return View();
        }

        public async Task<IActionResult> ResetPassword()
        {
            string token = HttpContext.Request.Query["token"].ToString();
            string result = await _authenticationServices.ValidatePasswordResetToken(token);
            if (result == Messages.SuccessMessage)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ResetPasswordExpired", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            string result = await _authenticationServices.ResetPassword(resetPasswordVM);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "Password reset successfully.";
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            string result = await _authenticationServices.RegisterUser(registerVM);
            if (result == Messages.SuccessMessage)
            {
                TempData["SuccessMessage"] = "User registered successfully.";
                return RedirectToAction("Login", "Authentication");
            }
            if (result == Messages.DuplicateValue)
            {
                TempData["ErrorMessage"] = "Email already taken!";
                return View();
            }
            return View();
        }
    }
}