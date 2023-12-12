namespace Domain.Interfaces;

public interface IOrderRepository
{
    void AddOrder(Order order);
    void SaveChanges();
}