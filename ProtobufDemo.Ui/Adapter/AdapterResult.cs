using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Ui.Adapter
{
    public class AdapterResult<T>
    {
        public T Data { get; set; }
        public long BytesReceived { get; set; }
        public long BytesSent { get; set; }
        public long ElapsedMilliseconds { get; set; }
    }
}
