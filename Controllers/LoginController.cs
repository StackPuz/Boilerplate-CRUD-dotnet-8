using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BC = BCrypt.Net.BCrypt;
using App.Models;

namespace App.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) {
                return LocalRedirect("/home");
            }
            if (Request.Query["ReturnUrl"].Any()) {
                HttpContext.Session.SetString("ReturnUrl", Request.Query["ReturnUrl"]);
            }
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Name, Password")] UserAccount login)
        {
            var user = await _context.UserAccount.FirstOrDefaultAsync(e => e.Name == login.Name);
            if (user != null && user.Active && BC.Verify(login.Password, user.Password)) {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                };
                var roles = _context.UserRole
                .Join(
                    _context.Role,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole, role }
                )
                .Where(e => e.userRole.UserId == user.Id)
                .Select(e => e.role.Name);
                foreach (var role in roles) {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                if (HttpContext.Session.GetString("ReturnUrl") != null) {
                    return LocalRedirect(HttpContext.Session.GetString("ReturnUrl"));
                }   
                return LocalRedirect("/home");
            }
            var message = (user != null && !user.Active ? "User is disabled" : "Invalid credentials");
            ViewData["error"] = message;
            return View();
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/Login");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return Ok("Access Denied.");
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPasswordPost()
        {
            var email = Request.Form["email"].First();
            var user = _context.UserAccount.FirstOrDefault(e => e.Email == email);
            if (user != null) {
                var token = Guid.NewGuid().ToString();
                user.PasswordResetToken = token;
                _context.SaveChanges();
                Util.SentMail("Reset", email, token);
                ViewData["success"] = true;
                return View("ResetPassword");
            }
            else {
                ViewData["error"] = true;
                return View("ResetPassword");
            }
        }
        
        [HttpGet("ChangePassword/{token}")]
        public IActionResult ChangePassword(string token)
        {
            var user = _context.UserAccount.FirstOrDefault(e => e.PasswordResetToken == token);
            if (user != null) {
                return View();
            }
            else {
                return NotFound();
            }
        }

        [HttpPost("ChangePassword/{token}")]
        public IActionResult ChangePasswordPost(string token)
        {
            var user = _context.UserAccount.FirstOrDefault(e => e.PasswordResetToken == token);
            if (user != null) {
                var password = Request.Form["password"].First();
                user.Password = BC.HashPassword(password);
                user.PasswordResetToken = null;
                _context.SaveChanges();
                ViewData["success"] = true;
                return View("ChangePassword");
            }
            else {
                ViewData["error"] = true;
                return View("ChangePassword");
            }
        }
    }
}