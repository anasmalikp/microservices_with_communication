using Dapper;
using Ecom.Data.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecom.Services.OrderServices.Service
{
    public interface IProdServices
    {
        Task<bool> CreateProduct(Products products);
    }

    public class ProdServices:IProdServices
    {
        private readonly IDbConnection connection;
        private readonly ILogger<ProdServices> logger;
        public ProdServices(IConfiguration config, ILogger<ProdServices> logger)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        public async Task<bool> CreateProduct(Products products)
        {
            try
            {
                var insert = await connection.ExecuteAsync("INSERT INTO products (prodName, stock, price, prodId) VALUES (@prodName, @stock, @price, @prodId)", new { prodName = products.prodName, stock = products.stock, price = products.price, prodId = products.id });
                if(insert > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
