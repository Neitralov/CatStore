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
        
        return result is not null ? result : Errors.Cat.NotFound;
    }
    
    public ErrorOr<IEnumerable<Cat>> GetCats()
    {
        var result = catRepository.GetCats();

        return result.ToList();
    }

    public ErrorOr<Updated> UpdateCatPrice(Guid catId, decimal price, decimal discount)
    {
        var cat = catRepository.FindCatById(catId);

        if (cat is null)
            return Errors.Cat.NotFound;

        var result = cat.UpdatePrice(price, discount);

        if (result == Result.Updated)
            catRepository.SaveChanges();
        
        return result;
    }
    
    public ErrorOr<Deleted> DeleteCat(Guid catId)
    {
        var result = catRepository.RemoveCat(catId);
        catRepository.SaveChanges();
        
        return result ? Result.Deleted : Errors.Cat.NotFound;
    }
}