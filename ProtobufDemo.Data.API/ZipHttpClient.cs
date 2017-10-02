using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Data.API
{
    public class ZipHttpClient : HttpClient
    {
        public ZipHttpClient(string baseAddress)
            : base(new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate })
        {
            this.BaseAddress = new Uri(baseAddress);
        }
    }
}
