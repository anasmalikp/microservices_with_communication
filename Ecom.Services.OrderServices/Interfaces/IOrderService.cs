using Ecom.Data.Models.Models;

namespace Ecom.Services.OrderServices.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddOrder(Orders orders);
        Task<IEnumerable<Orders>> GetOrders();
    }
}
