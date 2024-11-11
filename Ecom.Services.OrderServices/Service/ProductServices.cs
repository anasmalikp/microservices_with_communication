using Dapper;
using Ecom.Data.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecom.Services.OrderServices.Service
{
    public interface IProductServices
    {
        Task AddProduct(Products product);
    }
    public class ProductServices:IProductServices
    {
        private readonly IDbConnection connection;

        public ProductServices(IConfiguration config)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task AddProduct(Products product)
        {
            await connection.ExecuteAsync("INSERT INTO products (prodName, stock, price, prodId) VALUES (@prodName, @stock, @price, @prodId)", new { prodName = product.prodName, stock = product.stock, price = product.price, prodId = product.id });
        }
    }
}
