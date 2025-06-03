using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GreenCartFarmers.WebApp;
using GreenCartFarmers.WebApp.Data;

namespace GreenCartFarmers.WebApp.Controllers
{
    public class BuyerDeliveriesController : Controller
    {
        private GreenCartFarmersWebAppContext db = new GreenCartFarmersWebAppContext();

        public ActionResult Index(string username = null)
        {
            string loggedInUsername = username ?? User.Identity.Name;

            if (string.IsNullOrEmpty(loggedInUsername))
                return RedirectToAction("Login", "Auth");

            var loggedInUser = db.Users.FirstOrDefault(u => u.UserName == loggedInUsername);
            if (loggedInUser == null)
                return RedirectToAction("Login", "Auth");

            int buyerUserId = loggedInUser.UserId;

            var deliveries = db.Deliveries
                .Include(d => d.Order)
                .Where(d => d.Order.UserId == buyerUserId)
                .ToList();

            return View(deliveries);
        }


        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }

            // Check if delivery belongs to logged in buyer
            string loggedInUsername = User.Identity.Name;
            var loggedInUser = db.Users.FirstOrDefault(u => u.UserName == loggedInUsername);
            if (loggedInUser == null || delivery.Order.UserId != loggedInUser.UserId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(delivery);
        }

        // Buyer should not create or edit deliveries, so remove or restrict Create/Edit/Delete actions
        // If you want, you can override these to redirect buyers away or return Forbidden:

        // For example:
        public ActionResult Create()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Delivery delivery)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult Edit(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Delivery delivery)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult Delete(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
