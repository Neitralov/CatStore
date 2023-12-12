namespace Database;

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

    public void SaveChanges()
    {
        _database.SaveChanges();
    }
}