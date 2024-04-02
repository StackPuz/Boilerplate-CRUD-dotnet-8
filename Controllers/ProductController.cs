using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using ViewModel = App.ViewModels.Product;

namespace App.Controllers
{
    [Authorize(Roles = "ADMIN,USER")]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string column = Request.Query["sc"];
            if (Util.IsInvalidSearch(typeof(ViewModel.Index.Product), column)) {
                return Forbid();
            }
            int page = Request.Query["page"].Any() ? Convert.ToInt32(Request.Query["page"]) : 1;
            int size = Request.Query["size"].Any() ? Convert.ToInt32(Request.Query["size"]) : 10;
            string sort = Request.Query["sort"].Any() ? Request.Query["sort"].First() : "Id";
            bool sortDesc = Request.Query["sort"].Any() ? Request.Query["desc"].Any() : false;
            var query = _context.Product
            .SelectMany(
                product => _context.Brand.Where(brand => product.BrandId == brand.Id).DefaultIfEmpty(),
                (product, brand) => new { product, brand }
            )
            .SelectMany(
                combine => _context.UserAccount.Where(userAccount => combine.product.CreateUser == userAccount.Id).DefaultIfEmpty(),
                (combined, userAccount) => new { combined.product, combined.brand, userAccount }
            )
            .Select(e => new ViewModel.Index.Product {
                Id = e.product.Id,
                Image = e.product.Image,
                Name = e.product.Name,
                Price = e.product.Price,
                BrandName = e.brand.Name,
                UserAccountName = e.userAccount.Name
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
            var products = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            return View(products);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int? id)
        {
            var product = await _context.Product
            .SelectMany(
                product => _context.Brand.Where(brand => product.BrandId == brand.Id).DefaultIfEmpty(),
                (product, brand) => new { product, brand }
            )
            .SelectMany(
                combine => _context.UserAccount.Where(userAccount => combine.product.CreateUser == userAccount.Id).DefaultIfEmpty(),
                (combined, userAccount) => new { combined.product, combined.brand, userAccount }
            )
            .Select(e => new ViewModel.Detail.Product {
                Id = e.product.Id,
                Name = e.product.Name,
                Price = e.product.Price,
                BrandName = e.brand.Name,
                UserAccountName = e.userAccount.Name,
                Image = e.product.Image
            }).FirstOrDefaultAsync(e => e.Id == id);
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View(product);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewData["brands"] = await _context.Brand
            .Select(e => new ViewModel.Create.Brand {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Price, BrandId, Image, ImageFile")] ViewModel.Create.Product model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product();
                product.Name = model.Name;
                product.Price = model.Price;
                product.BrandId = model.BrandId;
                product.Image = Util.getFile("products", model.ImageFile);
                product.CreateUser = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                product.CreateDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
            }
            ViewData["brands"] = await _context.Brand
            .Select(e => new ViewModel.Create.Brand {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await _context.Product.FindAsync(id);
            var product = new ViewModel.Edit.Product {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                BrandId = item.BrandId,
                Image = item.Image
            };
            ViewData["brands"] = await _context.Brand
            .Select(e => new ViewModel.Edit.Brand {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View(product);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, BrandId, Image, ImageFile")] ViewModel.Edit.Product model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Product.FirstOrDefaultAsync(e => e.Id == id);
                    product.Name = model.Name;
                    product.Price = model.Price;
                    product.BrandId = model.BrandId;
                    product.Image = Util.getFile("products", model.ImageFile) ?? product.Image;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Product.Any(e => e.Id == id))
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
            ViewData["brands"] = await _context.Brand
            .Select(e => new ViewModel.Edit.Brand {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View(model);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Product
            .SelectMany(
                product => _context.Brand.Where(brand => product.BrandId == brand.Id).DefaultIfEmpty(),
                (product, brand) => new { product, brand }
            )
            .SelectMany(
                combine => _context.UserAccount.Where(userAccount => combine.product.CreateUser == userAccount.Id).DefaultIfEmpty(),
                (combined, userAccount) => new { combined.product, combined.brand, userAccount }
            )
            .Select(e => new ViewModel.Delete.Product {
                Id = e.product.Id,
                Name = e.product.Name,
                Price = e.product.Price,
                BrandName = e.brand.Name,
                UserAccountName = e.userAccount.Name,
                Image = e.product.Image
            }).FirstOrDefaultAsync(e => e.Id == id);
            ViewData["ref"] = Util.getRef(Request, "/Product");
            return View(product);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
        }
    }
}