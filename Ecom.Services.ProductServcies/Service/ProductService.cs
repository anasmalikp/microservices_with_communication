using Dapper;
using Ecom.Data.Models.Models;
using Ecom.Services.ProductServcies.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.Json;

namespace Ecom.Services.ProductServcies.Service
{
    public class ProductService:IProductService
    {
        private readonly IDbConnection connection;
        private readonly HttpClient client;

        public ProductService(IConfiguration config, HttpClient client)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.client = client;
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
                var data = JsonSerializer.Serialize(product);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = "https://localhost:7079/apigateway/orders/products";
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
