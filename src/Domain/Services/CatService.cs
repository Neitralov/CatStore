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
        
        return Result.Created;
    }
    
    public ErrorOr<Cat> GetCat(Guid catId)
    {
        var result = _repository.GetCat(catId);
        
        if (result is not null)
            return result;
        
        return Errors.Cat.NotFound;
    }
    
    public ErrorOr<Updated> UpdateCat(Cat cat)
    {
        var result = _repository.UpdateCat(cat);
        
        if (result)
            return Result.Updated;
        
        return Errors.Cat.NotFound;
    }
    
    public ErrorOr<Deleted> DeleteCat(Guid catId)
    {
        var result = _repository.DeleteCat(catId);
        
        if (result)
            return Result.Deleted;
        
        return Errors.Cat.NotFound;
    }
}