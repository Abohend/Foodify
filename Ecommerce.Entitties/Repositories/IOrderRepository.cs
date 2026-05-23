using Ecommerce.Entities.Models;

namespace Ecommerce.Entities.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        new void Update(Order order);
    }
}
