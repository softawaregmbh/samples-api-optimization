using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Manager
{
    public interface IOrderManager
    {
        Task<IEnumerable<Order>> GetOrdersAsync(bool includeLines = false);
    }
}
