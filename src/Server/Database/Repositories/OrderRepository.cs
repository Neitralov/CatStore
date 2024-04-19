namespace Database.Repositories;

public class OrderRepository(DatabaseContext database) : IOrderRepository
{
    public async Task AddOrder(Order order)
    {
        await database.AddAsync(order);
    }

    public async Task<Order?> GetOrder(Guid orderId, Guid userId)
    {
        return await database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .SingleOrDefaultAsync(order => 
                order.OrderId == orderId && 
                order.UserId == userId);
    }

    public async Task<List<Order>> GetOrders(Guid userId)
    {
        return await database.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();
    }

    public async Task SaveChanges() => await database.SaveChangesAsync();
}