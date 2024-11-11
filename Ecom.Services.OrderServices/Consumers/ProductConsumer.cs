using Dapper;
using Ecom.Data.Models.Models;
using Ecom.Services.OrderServices.kafka;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Ecom.Services.OrderServices.Consumers
{
    public class ProductConsumer:KafkaConsumer
    {
        public ProductConsumer(IConfiguration config, ILogger<ProductConsumer> logger): base(new SqlConnection(config.GetConnectionString("DefaultConnection")), logger)
        {
            
        }

        protected override string GetTopic()
        {
            return "product-topic";
        }

        protected override async Task ProcessMessageAsync(string message, CancellationToken stoppingToken)
        {
            var product = JsonConvert.DeserializeObject<Products>(message);
            if(product != null)
            {
                await connection.ExecuteAsync("INSERT INTO products (prodName, stock, price, prodId) VALUES (@prodName, @stock, @price, @prodId)", new { prodName = product.prodName, stock = product.stock, price = product.price, prodId = product.id });
            }
        }
    }
}
