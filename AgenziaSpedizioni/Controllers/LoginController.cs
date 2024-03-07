using AgenziaSpedizioni.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Spedizioni.Data;
using System.Security.Claims;

namespace AgenziaSpedizioni.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        public LoginController(ApplicationDbContext db, IAuthenticationSchemeProvider schemeProvider)
        {
            _db = db;
            _schemeProvider = schemeProvider;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            User? user = _db.Users.SingleOrDefault(u => u.Username == login.Username);

            if (user == null)
            {
                TempData["error"] = "Non esiste questo account";
                return View();
            }

            if (PasswordManager.VerifyPassword(login.Password, user.Password))
            {
                TempData["success"] = "Hai fatto il login";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Usertype),
                    new Claim(ClaimTypes.UserData, user.Id.ToString())
                };


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Password sbagliata";
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["success"] = "Sei stato disconnesso";

            return RedirectToAction("Index", "Home");
        }
    }
}
