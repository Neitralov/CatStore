namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/cats"), Tags("Cats")]
public class CatsController(CatService catService) : ApiController
{
    /// <summary>Добавить нового кота в магазин</summary>
    /// <response code="201">Кот создан</response>
    /// <response code="400">Неправильная длина имени, цвет указан в некорректном формате, цена указана некорректно, кот с таким именем уже существует</response>
    [HttpPost, Authorize("CanEditCats")]
    [ProducesResponseType(typeof(CatResponse), 201)]
    public async Task<IActionResult> CreateCat([Required] CreateCatRequest request)
    {
        ErrorOr<Cat> requestToCatResult = CreateCatFrom(request);
        
        if (requestToCatResult.IsError)
            return Problem(requestToCatResult.Errors);
        
        var cat = requestToCatResult.Value;
        ErrorOr<Created> createCatResult = await catService.StoreCat(cat);
        
        return createCatResult.Match(_ => CreatedAtGetCat(cat), Problem);
    }
    
    /// <summary>Получить данные о коте из магазина</summary>
    /// <param name="catId">Guid кота, которого нужно найти</param>
    /// <response code="200">Кот найден</response>
    /// <response code="404">Not found</response>
    [HttpGet("{catId:guid}")]
    [ProducesResponseType(typeof(CatResponse), 200)]
    public async Task<IActionResult> GetCat(Guid catId)
    {
        ErrorOr<Cat> getCatResult = await catService.GetCat(catId);
        
        return getCatResult.Match(cat => Ok(cat.Adapt<CatResponse>()), Problem);
    }
    
    /// <summary>Получить всех котов из магазина</summary>
    /// <response code="200">Список котов</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CatResponse>), 200)]
    public async Task<IActionResult> GetCats()
    {
        var cats = await catService.GetCats();

        return Ok(cats.Adapt<List<CatResponse>>());
    }

    /// <summary>Обновить цену существующего кота</summary>
    /// <param name="catId">Guid кота, цену которого нужно обновить</param>
    /// <param name="request"/>
    /// <response code="204">Цена успешно обновлена</response>
    /// <response code="400">Цена указана некорректно</response>
    /// <response code="404">Not found</response>
    [HttpPatch("{catId:guid}/update-price"), Authorize("CanEditCats")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateCatPrice(Guid catId, [Required] UpdateCatPriceRequest request)
    {
        ErrorOr<Updated> updateCatPriceResult = await catService.UpdateCatPrice(catId, request.Cost, request.Discount);
        
        return updateCatPriceResult.Match(_ => NoContent(), Problem);
    }
    
    /// <summary>Удалить кота из магазина</summary>
    /// <param name="catId">Guid кота, которого нужно удалить</param>
    /// <response code="204">Кот удален успешно</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{catId:guid}"), Authorize("CanEditCats")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCat(Guid catId)
    {
        ErrorOr<Deleted> deleteCatResult = await catService.DeleteCat(catId);
        
        return deleteCatResult.Match(_ => NoContent(), Problem);
    }
    
    private static ErrorOr<Cat> CreateCatFrom(CreateCatRequest request)
    {
        return Cat.Create(
            request.Name,
            request.SkinColor,
            request.EyeColor,
            request.EarColor,
            request.IsMale,
            request.Cost,
            request.Discount);
    }
    
    private CreatedAtActionResult CreatedAtGetCat(Cat cat)
    {
        return CreatedAtAction(
            actionName:  nameof(GetCat),
            routeValues: new { catId = cat.CatId },
            value:       cat.Adapt<CatResponse>());
    }
}