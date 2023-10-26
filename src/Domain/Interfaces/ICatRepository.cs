namespace Domain.Interfaces;

public interface ICatRepository
{
    void AddCat(Cat cat);
    Cat? GetCat(Guid id);
    bool UpdateCat(Cat cat);
    bool DeleteCat(Guid id);
}