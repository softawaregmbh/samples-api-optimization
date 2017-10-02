using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ProtobufDemo.Manager
{
    public interface IOrderManager
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> filter = null, params Expression<Func<Order, object>>[] includeProperties);
    }
}
