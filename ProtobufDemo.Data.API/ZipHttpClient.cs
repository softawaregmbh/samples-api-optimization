using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufDemo.Data.API
{
    public class ZipHttpClient : HttpClient
    {
        public ZipHttpClient(string baseAddress, SerializationStrategy serializationStrategy)
            : base(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
        {
            this.BaseAddress = new Uri(baseAddress);
            this.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue(serializationStrategy == SerializationStrategy.JSON ? "application/Json" : "application/x-protobuf"));
        }
    }
}
