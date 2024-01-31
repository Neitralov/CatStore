namespace Domain.Interfaces;

public interface IOrderRepository
{
    //Create
    void AddOrder(Order order);

    //Read
    Order? GetOrder(Guid orderId);
    IEnumerable<Order> GetAllUserOrders(Guid userId);

    //Update

    //Delete

    //Other
    void SaveChanges();
}