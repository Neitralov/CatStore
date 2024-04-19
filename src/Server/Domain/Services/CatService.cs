namespace Domain.Services;

public class CatService(ICatRepository catRepository)
{
    public async Task<ErrorOr<Created>> StoreCat(Cat cat)
    {
        if (await catRepository.IsCatExists(cat.Name))
            return Errors.Cat.AlreadyExists;

        await catRepository.AddCat(cat);
        await catRepository.SaveChanges();
        
        return Result.Created;
    }
    
    public async Task<ErrorOr<Cat>> GetCat(Guid catId)
    {
        var result = await catRepository.GetCat(catId);
        
        return result is not null ? result : Errors.Cat.NotFound;
    }
    
    public async Task<List<Cat>> GetCats()
    {
        var result = await catRepository.GetCats();

        return result;
    }

    public async Task<ErrorOr<Updated>> UpdateCatPrice(Guid catId, decimal price, decimal discount)
    {
        var cat = await catRepository.FindCatById(catId);

        if (cat is null)
            return Errors.Cat.NotFound;

        var result = cat.UpdatePrice(price, discount);

        if (result == Result.Updated)
            await catRepository.SaveChanges();
        
        return result;
    }
    
    public async Task<ErrorOr<Deleted>> DeleteCat(Guid catId)
    {
        var result = await catRepository.RemoveCat(catId);
        await catRepository.SaveChanges();
        
        return result ? Result.Deleted : Errors.Cat.NotFound;
    }
}