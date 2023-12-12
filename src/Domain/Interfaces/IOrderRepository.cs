namespace Domain.Interfaces;

public interface IOrderRepository
{
    void AddOrder(Order order);
    IEnumerable<Order> GetAllUserOrders(Guid userId);
    void SaveChanges();
}