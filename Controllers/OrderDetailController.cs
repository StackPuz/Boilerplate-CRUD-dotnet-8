using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.Models;
using ViewModel = App.ViewModels.OrderDetail;

namespace App.Controllers
{
    [Authorize(Roles = "ADMIN,USER")]
    [Route("[controller]")]
    public class OrderDetailController : Controller
    {
        private readonly DataContext _context;

        public OrderDetailController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewData["products"] = await _context.Product
            .Select(e => new ViewModel.Create.Product {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/OrderDetail");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId, No, ProductId, Qty")] ViewModel.Create.OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                var orderDetail = new OrderDetail();
                orderDetail.OrderId = model.OrderId;
                orderDetail.No = model.No;
                orderDetail.ProductId = model.ProductId;
                orderDetail.Qty = model.Qty;
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
            }
            ViewData["products"] = await _context.Product
            .Select(e => new ViewModel.Create.Product {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/OrderDetail");
            return View(model);
        }

        [HttpGet("Edit/{orderId}/{no}")]
        public async Task<IActionResult> Edit(int? orderId, short? no)
        {
            var item = await _context.OrderDetail.FindAsync(orderId, no);
            var orderDetail = new ViewModel.Edit.OrderDetail {
                OrderId = item.OrderId,
                No = item.No,
                ProductId = item.ProductId,
                Qty = item.Qty
            };
            ViewData["products"] = await _context.Product
            .Select(e => new ViewModel.Edit.Product {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/OrderDetail");
            return View(orderDetail);
        }

        [HttpPost("Edit/{orderId}/{no}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int orderId, short no, [Bind("OrderId, No, ProductId, Qty")] ViewModel.Edit.OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var orderDetail = await _context.OrderDetail.FirstOrDefaultAsync(e => e.OrderId == orderId && e.No == no);
                    orderDetail.ProductId = model.ProductId;
                    orderDetail.Qty = model.Qty;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OrderDetail.Any(e => e.OrderId == orderId && e.No == no))
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
            ViewData["products"] = await _context.Product
            .Select(e => new ViewModel.Edit.Product {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
            ViewData["ref"] = Util.getRef(Request, "/OrderDetail");
            return View(model);
        }

        [HttpGet("Delete/{orderId}/{no}")]
        public async Task<IActionResult> Delete(int? orderId, short? no)
        {
            var orderDetail = await _context.OrderDetail
            .SelectMany(
                orderDetail => _context.Product.Where(product => orderDetail.ProductId == product.Id).DefaultIfEmpty(),
                (orderDetail, product) => new { orderDetail, product }
            )
            .Select(e => new ViewModel.Delete.OrderDetail {
                OrderId = e.orderDetail.OrderId,
                No = e.orderDetail.No,
                ProductName = e.product.Name,
                Qty = e.orderDetail.Qty
            }).FirstOrDefaultAsync(e => e.OrderId == orderId && e.No == no);
            ViewData["ref"] = Util.getRef(Request, "/OrderDetail");
            return View(orderDetail);
        }

        [HttpPost("Delete/{orderId}/{no}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int orderId, short no)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(orderId, no);
            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return Redirect(WebUtility.UrlDecode(Request.Query["ref"].ToString()));
        }
    }
}