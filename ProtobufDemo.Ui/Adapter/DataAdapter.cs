using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ProtobufDemo.Manager;
using ProtobufDemo.Model;
using ProtobufDemo.Ui.Tracing;

namespace ProtobufDemo.Ui.Adapter
{
    public class DataAdapter : IDataAdapter
    {
        private IOrderManager manager;

        public DataAdapter(IOrderManager manager, string description)
        {
            this.manager = manager;
            this.Description = description;
        }

        public string Description { get; private set; }

        public async Task<AdapterResult<IEnumerable<Order>>> ReadDataAsync()
        {
            var result = new AdapterResult<IEnumerable<Order>>();
            TrafficTracer trafficTracer = null;
            using (trafficTracer = TrafficTracer.Create())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                result.Data = await this.manager.GetOrdersAsync(
                    null,
                    o => o.OrderLines,
                    o => o.Customer,
                    o => o.Salesperson);
                result.ElapsedMilliseconds = stopWatch.ElapsedMilliseconds;

                stopWatch.Stop();
            }

            result.BytesReceived = trafficTracer.ReceivedBytes;
            result.BytesSent = trafficTracer.SentBytes;

            return result;
        }
    }
}
