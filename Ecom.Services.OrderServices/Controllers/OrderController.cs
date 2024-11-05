using Ecom.Data.Models.Models;
using Ecom.Services.OrderServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Services.OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;
        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOrder(Orders order)
        {
            var response = await service.AddOrder(order);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await service.GetOrders();
            return Ok(response);
        }
    }
}
