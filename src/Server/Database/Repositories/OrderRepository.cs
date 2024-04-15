namespace Database.Repositories;

public class OrderRepository(DatabaseContext database) : IOrderRepository
{
    public void AddOrder(Order order)
    {
        database.Add(order);
    }

    public Order? GetOrder(Guid orderId, Guid userId)
    {
        return database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .SingleOrDefault(order => 
                order.OrderId == orderId && 
                order.UserId == userId);
    }

    public List<Order> GetOrders(Guid userId)
    {
        return database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.OrderDate)
            .ToList();
    }

    public void SaveChanges() => database.SaveChanges();
}