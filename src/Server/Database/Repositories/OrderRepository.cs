namespace Database.Repositories;

public class OrderRepository(IMongoDatabase database) : IOrderRepository
{
    public async Task AddOrder(Order order)
    {
        var collection = database.GetCollection<Order>("orders");
        
        await collection.InsertOneAsync(order);
    }

    public async Task<Order?> GetOrder(Guid orderId, Guid userId)
    {
        var collection = database.GetCollection<Order>("orders");
        
        var builder = Builders<Order>.Filter;
        var filter = builder.Eq(order => order.OrderId, orderId) & builder.Eq(order => order.UserId, userId);

        return await collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<List<Order>> GetOrders(Guid userId)
    {
        var collection = database.GetCollection<Order>("orders");
        
        var builder = Builders<Order>.Filter;
        var filter = builder.Eq(order => order.UserId, userId);
        
        return await collection.Find(filter).SortByDescending(order => order.OrderDate).ToListAsync();
    }

    public async Task SaveChanges() => await Task.CompletedTask;
}