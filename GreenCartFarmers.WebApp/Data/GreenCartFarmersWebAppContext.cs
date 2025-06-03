using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreenCartFarmers.WebApp.Data
{
    public class GreenCartFarmersWebAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GreenCartFarmersWebAppContext() : base("name=GreenCartFarmersWebAppContext")
        {
        }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.Product> Products { get; set; }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.User> Users { get; set; }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.Delivery> Deliveries { get; set; }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.OrderItem> OrderItems { get; set; }

        public System.Data.Entity.DbSet<GreenCartFarmers.WebApp.Review> Reviews { get; set; }
    }
}
