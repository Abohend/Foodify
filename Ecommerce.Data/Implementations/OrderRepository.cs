using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Implementations;
using Ecommerce.Entities.Models;
using Ecommerce.Entities.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataِAccess.Implementations
{
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _dbSet;
        public OrderRepository(Context context) : base(context)
        {
            _dbSet = context.Set<Order>();
        }
        public new void Update(Order order)
        {
            var entry = _dbSet.Entry(order);
            entry.State = EntityState.Modified;
            entry.Property(o => o.OrderStatus).IsModified = false;
        }
    }
}
