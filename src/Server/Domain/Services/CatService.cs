namespace Domain.Services;

public class CatService(ICatRepository catRepository)
{
    public ErrorOr<Created> StoreCat(Cat cat)
    {
        if (catRepository.IsCatExists(cat.Name))
            return Errors.Cat.AlreadyExists;

        catRepository.AddCat(cat);
        catRepository.SaveChanges();
        
        return Result.Created;
    }
    
    public ErrorOr<Cat> GetCat(Guid catId)
    {
        var result = catRepository.GetCat(catId);
        
        return result is { } ? result : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Updated> UpdateCat(Cat cat)
    {
        var result = catRepository.UpdateCat(cat);
        catRepository.SaveChanges();
        
        return result ? Result.Updated : Errors.Cat.NotFound;
    }
    
    public ErrorOr<Deleted> DeleteCat(Guid catId)
    {
        var result = catRepository.RemoveCat(catId);
        catRepository.SaveChanges();
        
        return result ? Result.Deleted : Errors.Cat.NotFound;
    }

    public ErrorOr<IEnumerable<Cat>> GetAllCats()
    {
        var result = catRepository.GetAllCats();
        
        return result.ToList();
    }
}