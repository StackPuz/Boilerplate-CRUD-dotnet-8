using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;
using App.Models;
using ViewModel = App.ViewModels.UserAccount;

namespace App.Controllers
{
    public class SystemController : Controller
    {
        private readonly DataContext _context;

        public SystemController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/home");
        }

        [Authorize]
        [HttpGet("home")]
        public IActionResult Home()
        {
            return View("/Views/Home.cshtml");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var id = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var item = _context.UserAccount.Find(id);
            var userAccount = new ViewModel.Edit.UserAccount {
                Name = item.Name,
                Email = item.Email,
                Password = ""
            };
            return View("/Views/Profile.cshtml", userAccount);
        }

        [Authorize]
        [HttpPost("updateProfile")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProfile([Bind("Name, Email, Password")] ViewModel.Edit.UserAccount model)
        {
            var id = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userAccount = _context.UserAccount.FirstOrDefault(e => e.Id == id);
            userAccount.Name = model.Name;
            userAccount.Email = model.Email;
            if (!String.IsNullOrEmpty(model.Password)) {
                userAccount.Password = BC.HashPassword(model.Password);
            }
            _context.SaveChangesAsync();
            return Redirect("/home");
        }

        [HttpGet("stack")]
        public IActionResult Stack()
        {
            return Ok(".NET MVC 8 + MySQL");
        }
    }
}