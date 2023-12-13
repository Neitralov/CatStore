namespace WebAPI.Controllers;

public class CatsController : ApiController
{
    private readonly CatService _catService;
    
    public CatsController(CatService catService)
    {
        _catService = catService;
    }
    
    [HttpPost, Authorize(Roles = "admin")]
    public IActionResult CreateCat(CreateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Created> createCatResult = _catService.StoreCat(cat);
        
        return createCatResult.Match(_ => CreatedAtGetCat(cat), Problem);
    }
    
    [HttpGet("{catId:guid}")]
    public IActionResult GetCat(Guid catId)
    {
        ErrorOr<Cat> getCatResult = _catService.GetCat(catId);
        
        return getCatResult.Match(cat => Ok(MapCatResponse(cat)), Problem);
    }
    
    [HttpPut("{catId:guid}"), Authorize(Roles = "admin")]
    public IActionResult UpdateCat(Guid catId, UpdateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(catId, request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Updated> upsertCatResult = _catService.UpdateCat(cat);
        
        return upsertCatResult.Match(_ => NoContent(), Problem);
    }
    
    [HttpDelete("{catId:guid}"), Authorize(Roles = "admin")]
    public IActionResult DeleteCat(Guid catId)
    {
        ErrorOr<Deleted> deleteCatResult = _catService.DeleteCat(catId);
        
        return deleteCatResult.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet]
    public IActionResult GetAllCats()
    {
        ErrorOr<IEnumerable<Cat>> getAllCatsResult = _catService.GetAllCats();

        return getAllCatsResult.Match(cats => Ok(new { Cats = cats.Select(MapCatResponse) } ), Problem);
    }
    
    private static ErrorOr<Cat> CreateCatFrom(CreateCatRequest request)
    {
        return Cat.Create(
            request.Name,
            request.SkinColor,
            request.EyeColor,
            request.IsMale,
            request.Cost);
    }
    
    private static ErrorOr<Cat> CreateCatFrom(Guid catId, UpdateCatRequest request)
    {
        return Cat.Create(
            request.Name,
            request.SkinColor,
            request.EyeColor,
            request.IsMale,
            request.Cost,
            catId);
    }

    private CreatedAtActionResult CreatedAtGetCat(Cat cat)
    {
        return CreatedAtAction(
            actionName:  nameof(GetCat),
            routeValues: new { catId = cat.CatId },
            value:       MapCatResponse(cat));
    }

    private static CatResponse MapCatResponse(Cat cat)
    {
        return new CatResponse(
            cat.CatId,
            cat.Name,
            cat.SkinColor,
            cat.EyeColor,
            cat.IsMale,
            cat.Cost);
    }
}