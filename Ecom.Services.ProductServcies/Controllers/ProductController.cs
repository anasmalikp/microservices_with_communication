using Ecom.Data.Models.Models;
using Ecom.Services.ProductServcies.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Services.ProductServcies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        public ProductController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await service.GetAllProducts();
            return Ok(response);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await service.GetById(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Products prod)
        {
            var response = await service.AddNewProduct(prod);
            if (response)
            {
                return Ok("product added");
            }
            return BadRequest("something went wrong");
        }
    }
}
