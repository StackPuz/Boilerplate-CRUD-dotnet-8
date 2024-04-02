using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;
using App.Models;
using ViewModel = App.ViewModels.UserAccount;

namespace App.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("[controller]")]
    public class UserAccountController : Controller
    {
        private readonly DataContext _context;

        public UserAccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string column = Request.Query["sc"];
            if (Util.IsInvalidSearch(typeof(ViewModel.Index.UserAccount), column)) {
                return Forbid();
            }
            int page = Request.Query["page"].Any() ? Convert.ToInt32(Request.Query["page"]) : 1;
            int size = Request.Query["size"].Any() ? Convert.ToInt32(Request.Query["size"]) : 10;
            string sort = Request.Query["sort"].Any() ? Request.Query["sort"].First() : "Id";
            bool sortDesc = Request.Query["sort"].Any() ? Request.Query["desc"].Any() : false;
            var query = _context.UserAccount
            .Select(e => new ViewModel.Index.UserAccount {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Active = e.Active
            });
            if (Request.Query["sw"].Any()) {
                string search = Request.Query["sw"];
                query = query.Where(column, Request.Query["so"], search);
            }
            query = query.OrderBy(sort, sortDesc);
            int count = await query.CountAsync();
            int last = (int)Math.Ceiling(count / (double)size);
            var paging = new Dictionary<string, int>();
            paging.Add("current", page);
            paging.Add("size", size);
            paging.Add("last", last);
            ViewData["paging"] = paging;
            var userAccounts = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return View(userAccounts);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int? id)
        {
            var userAccount = await _context.UserAccount
            .Select(e => new ViewModel.Detail.UserAccount {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Active = e.Active
            }).FirstOrDefaultAsync(e => e.Id == id);
            ViewData["userAccountUserRoles"] = await _context.UserAccount
            .Join(
                _context.UserRole,
                userAccount => userAccount.Id,
                userRole => userRole.UserId,
                (userAccount, userRole) => new { userAccount, userRole }
            )
            .Join(
                _context.Role,
                combine => combine.userRole.RoleId,
                role => role.Id,
                (combined, role) => new { combined.userAccount, combined.userRole, role }
            )
            .Where(e => e.userAccount.Id == id)
            .Select(e => new ViewModel.Detail.UserAccountUserRole {
                RoleName = e.role.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View(userAccount);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewData["roles"] = await _context.Role
            .Select(e => new ViewModel.Create.Role {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Email, Active")] ViewModel.Create.UserAccount model)
        {
            if (ModelState.IsValid)
            {
                var userAccount = new UserAccount();
                var token = Guid.NewGuid().ToString();
                userAccount.PasswordResetToken = token;
                userAccount.Password = BC.HashPassword(Guid.NewGuid().ToString().Substring(0, 10));
                userAccount.Name = model.Name;
                userAccount.Email = model.Email;
                userAccount.Active = model.Active;
                _context.Add(userAccount);
                await _context.SaveChangesAsync();
                var roles = Request.Form["RoleId"];
                if (roles.Any()) {
                    foreach (var role in roles) {
                        var userRole = new UserRole();
                        userRole.UserId = userAccount.Id;
                        userRole.RoleId = (dynamic)Convert.ChangeType(role, userRole.RoleId.GetType());
                        _context.UserRole.Add(userRole);
                    }
                }
                Util.SentMail("Welcome", userAccount.Email, token, userAccount.Name);
                await _context.SaveChangesAsync();
                return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
            }
            ViewData["roles"] = await _context.Role
            .Select(e => new ViewModel.Create.Role {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await _context.UserAccount.FindAsync(id);
            var userAccount = new ViewModel.Edit.UserAccount {
                Id = item.Id,
                Name = item.Name,
                Email = item.Email,
                Password = "",
                Active = item.Active
            };
            ViewData["userAccountUserRoles"] = await _context.UserAccount
            .Join(
                _context.UserRole,
                userAccount => userAccount.Id,
                userRole => userRole.UserId,
                (userAccount, userRole) => new { userAccount, userRole }
            )
            .Where(e => e.userAccount.Id == id)
            .Select(e => new ViewModel.Edit.UserAccountUserRole {
                RoleId = e.userRole.RoleId
            }).ToListAsync();
            ViewData["roles"] = await _context.Role
            .Select(e => new ViewModel.Edit.Role {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View(userAccount);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Email, Password, Active")] ViewModel.Edit.UserAccount model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAccount = await _context.UserAccount.FirstOrDefaultAsync(e => e.Id == id);
                    userAccount.Name = model.Name;
                    userAccount.Email = model.Email;
                    if (!String.IsNullOrEmpty(model.Password)) {
                        userAccount.Password = BC.HashPassword(model.Password);
                    }
                    userAccount.Active = model.Active;
                    _context.UserRole.RemoveRange(_context.UserRole.Where(e => e.UserId == model.Id));
                    var roles = Request.Form["RoleId"];
                    if (roles.Any()) {
                        foreach (var role in roles) {
                            var userRole = new UserRole();
                            userRole.UserId = model.Id;
                            userRole.RoleId = (dynamic)Convert.ChangeType(role, userRole.RoleId.GetType());
                            _context.UserRole.Add(userRole);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserAccount.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
            }
            ViewData["userAccountUserRoles"] = await _context.UserAccount
            .Join(
                _context.UserRole,
                userAccount => userAccount.Id,
                userRole => userRole.UserId,
                (userAccount, userRole) => new { userAccount, userRole }
            )
            .Where(e => e.userAccount.Id == id)
            .Select(e => new ViewModel.Edit.UserAccountUserRole {
                RoleId = e.userRole.RoleId
            }).ToListAsync();
            ViewData["roles"] = await _context.Role
            .Select(e => new ViewModel.Edit.Role {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View(model);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var userAccount = await _context.UserAccount
            .Select(e => new ViewModel.Delete.UserAccount {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Active = e.Active
            }).FirstOrDefaultAsync(e => e.Id == id);
            ViewData["userAccountUserRoles"] = await _context.UserAccount
            .Join(
                _context.UserRole,
                userAccount => userAccount.Id,
                userRole => userRole.UserId,
                (userAccount, userRole) => new { userAccount, userRole }
            )
            .Join(
                _context.Role,
                combine => combine.userRole.RoleId,
                role => role.Id,
                (combined, role) => new { combined.userAccount, combined.userRole, role }
            )
            .Where(e => e.userAccount.Id == id)
            .Select(e => new ViewModel.Delete.UserAccountUserRole {
                RoleName = e.role.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/UserAccount");
            return View(userAccount);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var userAccount = await _context.UserAccount.FindAsync(id);
            _context.UserAccount.Remove(userAccount);
            await _context.SaveChangesAsync();
            return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
        }
    }
}