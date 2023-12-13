namespace Domain.Services;

public class CatService
{
    private readonly ICatRepository _catRepository;
    
    public CatService(ICatRepository catRepository)
    {
        _catRepository = catRepository;
    }
    
    public ErrorOr<Created> StoreCat(Cat cat)
    {
        if (_catRepository.IsCatExists(cat.Name))
            return Errors.Cat.AlreadyExists;

        _catRepository.AddCat(cat);
        _catRepository.SaveChanges();
        
        return Result.Created;
    }
    
    public ErrorOr<Cat> GetCat(Guid catId)
    {
        var result = _catRepository.GetCat(catId);
        
        return result is { } ? result : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Updated> UpdateCat(Cat cat)
    {
        var result = _catRepository.UpdateCat(cat);
        _catRepository.SaveChanges();
        
        return result ? Result.Updated : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Deleted> DeleteCat(Guid catId)
    {
        var result = _catRepository.RemoveCat(catId);
        _catRepository.SaveChanges();
        
        return result ? Result.Deleted : Errors.Cat.NotFound;
    }

    public ErrorOr<IEnumerable<Cat>> GetAllCats()
    {
        var result = _catRepository.GetAllCats();
        
        return result.ToList();
    }
}