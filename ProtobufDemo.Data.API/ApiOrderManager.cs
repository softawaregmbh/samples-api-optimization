using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtobufDemo.Manager;
using ProtobufDemo.Model;
using Serialize.Linq.Extensions;

namespace ProtobufDemo.Data.API
{
    public class ApiOrderManager : IOrderManager
    {
        private ZipHttpClient client;
        private JsonMediaTypeFormatter requestFormatter;
        private MediaTypeWithQualityHeaderValue mediaTypeJson;

        public ApiOrderManager(string baseUrl)
        {
            this.client = new ZipHttpClient(baseUrl);
            this.requestFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            };

            this.mediaTypeJson = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> filter = null, params Expression<Func<Order, object>>[] includeProperties)
        {
            var url = $"/api/orders/filterInclude";
            var request = new FilterIncludeRequest(filter?.ToExpressionNode(), includeProperties?.Select(p => p.ToExpressionNode()).ToArray());
            var response = await client.PostAsync(url, request, requestFormatter, mediaTypeJson, CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                return await this.Deserialize<IEnumerable<Order>>(response);
            }
            else
            {
                throw new Exception($"StatusCode: {response.StatusCode}");
            }
        }

        private async Task<TValue> Deserialize<TValue>(HttpResponseMessage message)
        {
            try
            {
                var stringContent = await message.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TValue>(stringContent);
            }
            catch (Exception)
            {
                return default(TValue);
            }
        }
    }
}
