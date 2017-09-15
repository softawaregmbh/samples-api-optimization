using ProtobufDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Ui.Adapter
{
    public interface IDataAdapter
    {
        string Description { get; }
        Task<AdapterResult<IEnumerable<Order>>> ReadDataAsync();
    }
}
