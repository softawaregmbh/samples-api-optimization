using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Data.EF
{
    public class DemoContext : DbContext
    {
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderLine> OrderLines { get; set; }

        public DemoContext()
            : base("name=EntityFramework")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DemoContext(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DemoContext>(null);
            modelBuilder.Entity<Order>().ToTable("Sales.Orders");
            modelBuilder.Entity<OrderLine>().ToTable("Sales.OrderLines");
            base.OnModelCreating(modelBuilder);
        }
    }
}
