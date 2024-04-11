namespace Domain.Interfaces;

public interface IOrderRepository
{
    //Create
    void AddOrder(Order order);

    //Read
    Order? GetOrder(Guid orderId, Guid userId);
    IEnumerable<Order> GetOrders(Guid userId);

    //Update

    //Delete

    //Other
    void SaveChanges();
}