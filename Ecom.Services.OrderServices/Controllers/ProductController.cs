using Ecom.Data.Models.Models;
using Ecom.Services.OrderServices.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Services.OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProdServices services;
        public ProductController(IProdServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> createProduct(Products prod)
        {
            var response = await services.CreateProduct(prod);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
