namespace Domain.Interfaces;

public interface IOrderRepository
{
    //Create
    Task AddOrder(Order order);

    //Read
    Task<Order?> GetOrder(Guid orderId, Guid userId);
    Task<List<Order>> GetOrders(Guid userId);

    //Update

    //Delete

    //Other
    Task SaveChanges();
}