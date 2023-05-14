using WebService.Model;

namespace WebService.Services;

public interface IOrdersRepository
{
    IEnumerable<Order> GetAllOrders();
    Order? GetOrderByID(int? id);
    Order CreateNewOrder(Order newOrder);
}
