namespace Domain.Interfaces;

public interface IOrderRepository
{
    void AddOrder(Order order);
    Order? GetOrder(Guid orderId);
    IEnumerable<Order> GetAllUserOrders(Guid userId);
    void SaveChanges();
}