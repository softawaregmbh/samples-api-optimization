using ProtobufDemo.Manager;
using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Data.EF.Manager
{
    public class OrderManager : IOrderManager
    {
        private Func<DemoContext> contextFactory;

        public OrderManager(Func<DemoContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(bool includeLines = false)
        {
            using (var context = this.contextFactory())
            {
                var query = context.Orders.AsQueryable();
                if (includeLines)
                {
                    query = query.Include(o => o.OrderLines);
                }

                return await query.ToListAsync();
            }
        }
    }
}
