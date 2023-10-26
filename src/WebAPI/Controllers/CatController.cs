namespace WebAPI.Controllers;

public class CatController : ApiController
{
    private readonly CatService _catService;
    
    public CatController(CatService catService)
    {
        _catService = catService;
    }
    
    [HttpPost]
    [Route("/cats")]
    public IActionResult CreateCat(CreateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Created> createCatResult = _catService.StoreCat(cat);
        
        return createCatResult.Match(_ => CreatedAtGetCat(cat), Problem);
    }
    
    [HttpGet]
    [Route("/cats/{id:guid}")]
    public IActionResult GetCat(Guid id)
    {
        ErrorOr<Cat> getCatResult = _catService.GetCat(id);
        
        return getCatResult.Match(cat => Ok(MapCatResponse(cat)), Problem);
    }
    
    [HttpPatch]
    [Route("/cats/{id:guid}")]
    public IActionResult UpdateCat(Guid id, UpdateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(id, request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Updated> updatedCatResult = _catService.UpdateCat(cat);
        
        return updatedCatResult.Match(_ => NoContent(), Problem);
    }
    
    [HttpDelete]
    [Route("/cats/{id:guid}")]
    public IActionResult DeleteCat(Guid id)
    {
        ErrorOr<Deleted> deletedCatResult = _catService.DeleteCat(id);
        
        return deletedCatResult.Match(_ => NoContent(), Problem);
    }
    
    private static CatResponse MapCatResponse(Cat cat)
    {
        return new CatResponse(
            cat.Id,
            cat.Name,
            cat.SkinColor,
            cat.EyeColor,
            cat.IsMale,
            cat.Cost);
    }
    
    private CreatedAtActionResult CreatedAtGetCat(Cat cat)
    {
        return CreatedAtAction(
            actionName: nameof(GetCat),
            routeValues: new { id = cat.Id },
            value: MapCatResponse(cat));
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
    
    private static ErrorOr<Cat> CreateCatFrom(Guid id, UpdateCatRequest request)
    {
        return Cat.Create(
            request.Name,
            request.SkinColor,
            request.EyeColor,
            request.IsMale,
            request.Cost,
            id);
    }
}