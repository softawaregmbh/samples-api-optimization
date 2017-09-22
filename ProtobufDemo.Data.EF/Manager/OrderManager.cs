using ProtobufDemo.Manager;
using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ProtobufDemo.Data.EF.Manager
{
    public class OrderManager : IOrderManager
    {
        private Func<DemoContext> contextFactory;

        public OrderManager(Func<DemoContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> filter, params Expression<Func<Order, object>>[] includeProperties)
        {
            using (var context = this.contextFactory())
            {
                var query = context.Orders.AsQueryable();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (includeProperties != null)
                {
                    foreach (var include in includeProperties)
                    {
                        query = query.Include(include);
                    }
                }

                return await query.ToListAsync();
            }
        }
    }
}
