namespace Database.Repositories;

public class OrderRepository(IDbContextFactory<DatabaseContext> factory) : IOrderRepository
{
    private DatabaseContext Database { get; } = factory.CreateDbContext();
    
    public void AddOrder(Order order)
    {
        Database.Add(order);
    }

    public Order? GetOrder(Guid orderId)
    {
        return Database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .SingleOrDefault(order => order.OrderId == orderId);
    }

    public IEnumerable<Order> GetAllUserOrders(Guid userId)
    {
        return Database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.OrderDate);
    }

    public void SaveChanges() => Database.SaveChanges();
}