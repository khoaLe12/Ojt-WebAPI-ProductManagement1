using Microsoft.EntityFrameworkCore;
using WebService.Model;

namespace WebService.Services;

public class OrdersRepository : IOrdersRepository
{
    private readonly NorthwindContext context;

    public OrdersRepository(NorthwindContext context)
    {
        this.context = context;
    }

    public IEnumerable<Order> GetAllOrders() => context.Orders.AsNoTracking().ToArray();

    public Order? GetOrderByID(int? id)
    {
        return context.Orders.AsNoTracking()
            .Include(o => o.OrderDetails)
            .Include(nameof(Order.OrderDetails) + "." + nameof(OrderDetail.Product))
            .FirstOrDefault(o => o.OrderId == id);
    }

    public Order CreateNewOrder(Order newOrder)
    {
        context.Add(newOrder);
        context.SaveChanges();
        return newOrder;
    }
}
