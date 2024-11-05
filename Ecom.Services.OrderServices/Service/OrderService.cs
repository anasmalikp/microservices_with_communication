using Confluent.Kafka;
using Dapper;
using Ecom.Data.Models.Models;
using Ecom.Services.OrderServices.Interfaces;
using Ecom.Services.OrderServices.kafka;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Ecom.Services.OrderServices.Service
{
    public class OrderService:IOrderService
    {
        private readonly IDbConnection connection;
        private readonly IKafkaProducer producer;
        public OrderService(IConfiguration config, IKafkaProducer producer)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.producer = producer;
        }

        public async Task<bool> AddOrder(Orders orders)
        {
            var product = await connection.QueryFirstOrDefaultAsync<Orderproducts>("SELECT * FROM products WHERE prodId = @id", new { id = orders.id });
            if (product == null && product.stock < orders.qty)
            {
                return false;
            }
            orders.totPrice = product.price * orders.qty;
            orders.prodName = product.prodName;

            var insert = await connection.ExecuteAsync("INSERT INTO orders (prodName, qty, totprice, customer) VALUES (@prodName, @qty, @totPrice, @customer)", new { prodName = orders.prodName, qty = orders.qty, totPrice = orders.totPrice, customer = orders.customer });
            await producer.ProduceAsync("order-topic", new Message<string, string>
            {
                Key = orders.id.ToString(),
                Value = JsonConvert.SerializeObject(orders)
            });
            await connection.ExecuteAsync("UPDATE products SET stock = stock - @qty WHERE prodId = @id", new {qty=orders.qty, id= orders.id});
            return true;
        }

        public async Task<IEnumerable<Orders>> GetOrders()
        {
            var orders = await connection.QueryAsync<Orders>("SELECT * FROM orders");
            return orders;
        }
    }
}
