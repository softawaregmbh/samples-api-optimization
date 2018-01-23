using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProtobufDemo.Data.EF.Manager;
using ProtobufDemo.Manager;
using ProtobufDemo.Data.API;
using ProtobufDemo.Model;

namespace ProtobufDemo.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private IOrderManager manager;

        public OrdersController(IOrderManager manager)
        {
            this.manager = manager;
        }

        [HttpPost("filterInclude")]
        public async Task<IActionResult> FilterIncludeAsync([FromBody] FilterIncludeRequest request)
        {
            try
            {
                var query = request.Filter?.ToBooleanExpression<Order>();
                var includes = request.IncludeProperties?.Select(en => en.ToExpression<Func<Order, object>>()).ToArray();

                var result = await this.manager.GetOrdersAsync(query, includes);
                return new ObjectResult(result);
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }

    }
}
