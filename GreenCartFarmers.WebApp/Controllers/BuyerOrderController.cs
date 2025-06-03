using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GreenCartFarmers.WebApp.Data;
using System.Data.SqlClient; // Required


namespace GreenCartFarmers.WebApp.Controllers
{
    public class BuyerOrderController : Controller
    {
        private GreenCartFarmersWebAppContext db = new GreenCartFarmersWebAppContext();

        // GET: BuyerOrder/Create — Show product list for selection
        // GET: BuyerOrder/Create — Show product list for selection
        public ActionResult Create()
        {
            var products = db.Products.ToList();

            // Ensure null safety (though ToList() generally avoids this)
            if (products == null)
                products = new List<Product>();

            return View(products);  // Pass product list to Create.cshtml
        }


        // POST: BuyerOrder/Create — Process and save order + items
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            var selectedProductIds = form.GetValues("productIds")?.Select(int.Parse).ToList();
            var quantities = form.GetValues("quantities")?.Select(int.Parse).ToList();
            string username = form["username"];

            if (selectedProductIds == null || !selectedProductIds.Any())
            {
                TempData["Error"] = "❌ Please select at least one product.";
                return RedirectToAction("Create");
            }

            if (string.IsNullOrEmpty(username))
            {
                TempData["Error"] = "❌ Buyer username is required.";
                return RedirectToAction("Create");
            }

            // Try to find existing user
            var user = db.Users.FirstOrDefault(u => u.UserName == username);

            // Optionally auto-register new buyer
            if (user == null)
            {
                user = new User
                {
                    UserName = username,
                    Role = "Buyer",
                    CreatedAt = DateTime.Now
                };
                db.Users.Add(user);
                db.SaveChanges();
            }

            // Create Order
            Order order = new Order
            {
                UserId = user.UserId,
                OrderDate = DateTime.Now
            };
            db.Orders.Add(order);
            db.SaveChanges();

            // Add Order Items with quantity
            for (int i = 0; i < selectedProductIds.Count; i++)
            {
                var item = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = selectedProductIds[i],
                    Quantity = quantities[i]
                };
                db.OrderItems.Add(item);
            }

            db.SaveChanges();
            TempData["Success"] = "🎉 Order placed successfully!";
            return RedirectToAction("Index", new { username = username });
        }


        public ActionResult Index(string username)
        {
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth");

            var buyer = db.Users.FirstOrDefault(u => u.UserName == username);
            if (buyer == null)
                return RedirectToAction("Login", "Auth");

            var orders = db.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == buyer.UserId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // GET: BuyerOrder/Details/5 — Order details page
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.Orders
                .Include(o => o.OrderItems.Select(oi => oi.Product))
                .FirstOrDefault(o => o.OrderId == id);

            var buyer = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (order == null || buyer == null || order.UserId != buyer.UserId)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
