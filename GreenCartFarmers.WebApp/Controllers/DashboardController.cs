using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenCartFarmers.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult FarmerDashboard()
        {
            return View();
        }

        public ActionResult BuyerDashboard()
        {
            return View();
        }
    }
}