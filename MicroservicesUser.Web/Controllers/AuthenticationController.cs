using Microservices.Common.ResourcesFiles;
using MicroservicesUser.BusinessLogic.Interfaces;
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
            if(User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Dashboard","Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            string token = await _authenticationServices.LoginUser(loginVM);
            if(token == Messages.AuthenticationFailed)
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
                return RedirectToAction("Dashboard","Index");
            }
            
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
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