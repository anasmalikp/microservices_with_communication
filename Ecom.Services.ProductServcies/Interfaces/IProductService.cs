using Ecom.Data.Models.Models;

namespace Ecom.Services.ProductServcies.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Products>> GetAllProducts();
        Task<Products> GetById(int id);
        Task<bool> AddNewProduct(Products product);
    }
}
