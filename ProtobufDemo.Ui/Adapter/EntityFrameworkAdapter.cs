using ProtobufDemo.Data.EF.Manager;
using ProtobufDemo.Model;
using ProtobufDemo.Ui.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Ui.Adapter
{
    public class EntityFrameworkAdapter : IDataAdapter
    {
        private OrderManager manager;

        public EntityFrameworkAdapter()
        {
            this.manager = new OrderManager(() => new Data.EF.DemoContext());
        }

        public string Description => "direct EntityFramework Connection";

        public async Task<AdapterResult<IEnumerable<Order>>> ReadDataAsync()
        {
            var result = new AdapterResult<IEnumerable<Order>>();
            TrafficTracer trafficTracer = null;
            using (trafficTracer = TrafficTracer.Create())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                result.Data = await this.manager.GetOrdersAsync(true);
                result.ElapsedMilliseconds = stopWatch.ElapsedMilliseconds;

                stopWatch.Stop();
            }

            result.BytesReceived = trafficTracer.ReceivedBytes;
            result.BytesSent = trafficTracer.SentBytes;

            return result;
        }
    }
}
