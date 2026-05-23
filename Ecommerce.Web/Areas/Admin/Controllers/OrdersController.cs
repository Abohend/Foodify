using Ecommerce.Entities.Models;
using Ecommerce.Entities.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CustomRoles.admin)]

    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllOrders()
        {
            var orders = _unitOfWork.Order.GetAll();
            return Json(new { data = orders });
        }
        public IActionResult Details(int id)
        {
            var order = _unitOfWork.Order.GetOne(o => o.Id == id, includeEntities: "OrderItems.Product");
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails(Order order)
        {
            _unitOfWork.Order.Update(order);
            _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessOrder(Order order)
        {
            order.OrderStatus = Status.Processing;
            _unitOfWork.Order.Update(order);
            _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder(Order order)
        {
            // Update tracking number, carrier, shipping date and order status
            order.ShippingDate = DateTime.Now;
            if (order.Carrier == null)
            {
                ModelState.AddModelError("Carrier", "Carrier is required");
                var newOrder = _unitOfWork.Order.GetOne(o => o.Id == order.Id, "OrderItems.Product");
                return View("Details", new { id = order.Id });
            }
            else if (order.TrackingNumber == null)
            {
                ModelState.AddModelError("TrackingNumber", "Tracking number is required");
                var newOrder = _unitOfWork.Order.GetOne(o => o.Id == order.Id, "OrderItems.Product");
                return View("Details", new { id = order.Id });
            } 
            order.OrderStatus = Status.Shipped;
            order.ShippingDate = DateTime.Now;
            _unitOfWork.Order.Update(order);
            _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = order.Id });
        }
    }
}
