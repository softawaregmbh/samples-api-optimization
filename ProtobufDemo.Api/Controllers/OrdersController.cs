using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProtobufDemo.Data.EF.Manager;
using ProtobufDemo.Manager;

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

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var orders = await this.manager.GetOrdersAsync();
            return this.Ok(orders);
        }
    }
}
