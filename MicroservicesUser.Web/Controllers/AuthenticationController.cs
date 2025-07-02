using MicroservicesUser.BusinessLogic.Interfaces;
using MicroservicesUser.Common.ResourcesFiles;
using MicroservicesUser.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesUser.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IConfiguration _configuration;
        public AuthenticationController(IAuthenticationServices authenticationServices, IConfiguration configuration)
        {
            _authenticationServices = authenticationServices;
            _configuration = configuration;
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
                double hours = Convert.ToDouble(_configuration["AuthTokenExpiryTime:Hours"]);
                DateTime expiresAt = DateTime.Now.AddHours(hours);
                CookieOptions cookieOptions = new()
                {
                    HttpOnly = true,
                    Expires = expiresAt,
                };
                Response.Cookies.Append("AuthToken", token, cookieOptions);
                TempData["SuccessMessage"] = "Logged in successful.";
                return RedirectToAction("Index", "Dashboard");
            }
        }
        public IActionResult ForgotPassword()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Dashboard");
            }
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
                TempData["ErrorMessage"] = "No account found associated with this email.";
            }
            return View();
        }

        public async Task<IActionResult> ResetPassword()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("ResetPasswordExpired", "Error");
            }
            else
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

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login", "Authentication");
        }
    }
}