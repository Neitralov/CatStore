namespace WebAPI.Controllers;

/// <inheritdoc />
public class CatsController : ApiController
{
    private readonly CatService _catService;
    
    /// <inheritdoc />
    public CatsController(CatService catService)
    {
        _catService = catService;
    }
    
    /// <summary>Добавить нового кота в магазин (требуемая роль = админ)</summary>
    /// <response code="201">Кот создан</response>
    /// <response code="400">Неправильная длина имени, цвет указан в некорректном формате, цена указана некорректно, кот с таким именем уже существует</response>
    [HttpPost, Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(CatResponse), 201)]
    public IActionResult CreateCat([Required] CreateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Created> createCatResult = _catService.StoreCat(cat);
        
        return createCatResult.Match(_ => CreatedAtGetCat(cat), Problem);
    }
    
    /// <summary>Получить данные о коте из магазина</summary>
    /// <param name="catId">Guid кота, которого нужно найти</param>
    /// <response code="200">Кот найден</response>
    /// <response code="404">Not found</response>
    [HttpGet("{catId:guid}")]
    [ProducesResponseType(typeof(CatResponse), 200)]
    public IActionResult GetCat(Guid catId)
    {
        ErrorOr<Cat> getCatResult = _catService.GetCat(catId);
        
        return getCatResult.Match(cat => Ok(MapCatResponse(cat)), Problem);
    }
    
    /// <summary>Получить всех котов из магазина</summary>
    /// <response code="200">Список котов</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CatResponse>), 200)]
    public IActionResult GetAllCats()
    {
        ErrorOr<IEnumerable<Cat>> getAllCatsResult = _catService.GetAllCats();

        return getAllCatsResult.Match(cats => Ok(new List<CatResponse>(cats.Select(MapCatResponse))), Problem);
    }

    /// <summary>Обновить данные существующего кота (требуемая роль = админ)</summary>
    /// <param name="catId">Guid кота, данные которого нужно обновить</param>
    /// <param name="request"/>
    /// <response code="204">Данные успешно обновлены</response>
    /// <response code="400">Неправильная длина имени, цвет указан в некорректном формате, цена указана некорректно, кот с таким именем уже существует</response>
    /// <response code="404">Not found</response>
    [HttpPut("{catId:guid}"), Authorize(Roles = "admin")]
    [ProducesResponseType(204)]
    public IActionResult UpdateCat(Guid catId, [Required] UpdateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(catId, request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Updated> upsertCatResult = _catService.UpdateCat(cat);
        
        return upsertCatResult.Match(_ => NoContent(), Problem);
    }
    
    /// <summary>Удалить кота из магазина (требуемая роль = админ)</summary>
    /// <param name="catId">Guid кота, которого нужно удалить</param>
    /// <response code="204">Кот удален успешно</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{catId:guid}"), Authorize(Roles = "admin")]
    [ProducesResponseType(204)]
    public IActionResult DeleteCat(Guid catId)
    {
        ErrorOr<Deleted> deleteCatResult = _catService.DeleteCat(catId);
        
        return deleteCatResult.Match(_ => NoContent(), Problem);
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