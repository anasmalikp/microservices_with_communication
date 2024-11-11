using Confluent.Kafka;
using Dapper;
using Ecom.Data.Models.Models;
using Ecom.Services.ProductServcies.Interfaces;
using Ecom.Services.ProductServcies.kafka;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Ecom.Services.ProductServcies.Service
{
    public class ProductService:IProductService
    {
        private readonly IDbConnection connection;
        private readonly IKafkaPublisher publisher;

        public ProductService(IConfiguration config, IKafkaPublisher publisher)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.publisher = publisher;
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            var products = await connection.QueryAsync<Products>("SELECT * FROM products");
            return products;
        }

        public async Task<Products> GetById(int id)
        {
            var product = await connection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM products WHERE id = @id", new { id = id });
            return product;
        }

        public async Task<bool> AddNewProduct(Products product)
        {
            var insert = await connection.ExecuteScalarAsync<int>("INSERT INTO products (prodName, stock, price) VALUES (@prodName, @stock, @price) SELECT CAST(SCOPE_IDENTITY() AS int);", new {prodName=product.prodName, stock=product.stock, price=product.price});
            if(insert>0)
            {
                product.id = insert;
                await publisher.ProduceAsync("product-topic", new Message<string, string>
                {
                    Key = insert.ToString(),
                    Value = JsonConvert.SerializeObject(product)
                });
                return true;
            }
            return false;
        }
    }
}
