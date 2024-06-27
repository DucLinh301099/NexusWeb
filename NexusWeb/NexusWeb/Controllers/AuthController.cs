using AutoMapper;
using NexusWeb.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NexusWeb.Models;
using NexusWeb.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Drawing;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace NexusWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly NexusWebAppContext _appContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, NexusWebAppContext appContext, IMapper mapper )
        {
            _mapper = mapper;
            _logger = logger;
            _appContext = appContext;
        }

        public const string SessionAge = "IdUser";

		#region Login
		[HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var user = _appContext.Users.SingleOrDefault(u => u.UserName == model.UserName);
                if (user == null)
                {
                    ViewData["Message"] = "Username or password is incorrect !";
                }
                else
                {
                    if (user.Status == 0)
                    {
                        ViewData["Message"] = "Account has been locked. Please contact Admin!";
                    }
                    else
                    {
                        if (user.Password != model.Password.ToMd5Hash(user.RandomKey))
                        {
                            ViewData["Message"] = "Username or password is incorrect !";
                        }
                        else
                        {
                            var level=user.Level;
                            string role = "Customer";
                            switch (level)
                            {
                                case 0:
                                    role = "Customer";
                                    break;
                                case 1:
                                    role = "Retail Store Employee";
                                    break;
                                case 2:
                                    role = "Technical staff";
                                    break;
                                case 3:
                                    role = "Accounts Department Officer";
                                    break;
                                case 4:
                                    role = "Admin";
                                    break;
                            }

                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, user.UserName),
								//claim - role động
								new Claim(ClaimTypes.Role, role),
                            };
                            HttpContext.Session.SetInt32("IdUser", user.Id);
							HttpContext.Session.SetString("UserName", user.UserName);
							HttpContext.Session.SetInt32("Level", user.Level);
							HttpContext.Session.SetString("Role", role);

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                if (user.Level == 0)
                                {
                                    return RedirectToAction("AccountD", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Admin");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }

        #endregion
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model) {
            if (ModelState.IsValid)
            {
                var check = _appContext.Users.FirstOrDefault(c => c.UserName.ToUpper().Equals(model.UserName.ToUpper()));
                if (check != null)
                {
                    ViewData["Message"] = "Email already exists.";
                    return View();
                }
                var RandomKey = MyUtil.GenerateRamdomKey();
                var user = new User
                {
                    UserName = model.UserName,
                    Password = model.Password.ToMd5Hash(RandomKey),
                    Level = 0,
                    Status = 1,
                    RandomKey = RandomKey,
                    FullName = "",
                    Address = "",
                    Phone = "",
                    Image = "",
                };

                _appContext.Users.Add(user);
                await _appContext.SaveChangesAsync();
                


                return RedirectToAction("AccountD", "Home");
            }
            return View();
            
        }
        #endregion
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
