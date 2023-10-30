namespace Domain.Services;

public class CatService
{
    private readonly ICatRepository _repository;
    
    public CatService(ICatRepository repository)
    {
        _repository = repository;
    }
    
    public ErrorOr<Created> StoreCat(Cat cat)
    {
        _repository.AddCat(cat);
        _repository.SaveChanges();
        
        return Result.Created;
    }
    
    public ErrorOr<Cat> GetCat(Guid catId)
    {
        var result = _repository.GetCat(catId);
        
        return result is { } ? result : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Updated> UpdateCat(Cat cat)
    {
        var result = _repository.UpdateCat(cat);
        _repository.SaveChanges();
        
        return result ? Result.Updated : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Deleted> DeleteCat(Guid catId)
    {
        var result = _repository.DeleteCat(catId);
        _repository.SaveChanges();
        
        return result ? Result.Deleted : Errors.Cat.NotFound;
    }

    public ErrorOr<IEnumerable<Cat>> GetAllCats()
    {
        var result = _repository.GetAllCats();
        
        return result.ToList();
    }
}