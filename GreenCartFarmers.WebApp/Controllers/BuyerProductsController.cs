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
    public class BuyerProductsController : Controller
    {
        private GreenCartFarmersWebAppContext db = new GreenCartFarmersWebAppContext();

        // GET: BuyerProducts
        // List all products for buyers (read-only)
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        // GET: BuyerProducts/Details/5
        // Show details of a single product
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
