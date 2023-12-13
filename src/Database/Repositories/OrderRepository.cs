namespace Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _database;

    public OrderRepository(IDbContextFactory<DatabaseContext> factory)
    {
        _database = factory.CreateDbContext();
    }
    
    public void AddOrder(Order order)
    {
        _database.Add(order);
    }

    public Order? GetOrder(Guid orderId)
    {
        return _database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .SingleOrDefault(order => order.OrderId == orderId);
    }

    public IEnumerable<Order> GetAllUserOrders(Guid userId)
    {
        return _database.Orders
            .AsNoTracking()
            .Where(order => order.UserId == userId);
    }

    public void SaveChanges() => _database.SaveChanges();
}