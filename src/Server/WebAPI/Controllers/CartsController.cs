namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/cart-items"), Tags("CartItems")]
public class CartsController(CartService cartService) : ApiController
{
    /// <summary>Добавить товар в корзину пользователя</summary>
    /// <response code="204">Товар успешно добавлен в корзину</response>
    /// <response code="400">У товара указано количество меньше единицы</response>
    /// <response code="404">Not found</response>
    [HttpPost, Authorize]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CreateCartItem([Required] CreateCartItemRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> requestToCartItemResult = CreateCartItemFrom(request, userId);

        if (requestToCartItemResult.IsError)
            return Problem(requestToCartItemResult.Errors);

        var cartItem = requestToCartItemResult.Value;
        ErrorOr<Created> createCartItemResult = await cartService.StoreCartItem(cartItem);

        return createCartItemResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Получить товар из корзины пользователя</summary>
    /// <param name="catId">Guid кота, который должен быть в корзине</param>
    /// <response code="200">Товар из корзины найден</response>
    /// <response code="404">Not found</response>
    [HttpGet("{catId:guid}"), Authorize]
    [ProducesResponseType(typeof(CartItemResponse), 200)]
    public async Task<IActionResult> GetCartItem(Guid catId)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> getCartItemResult = await cartService.GetCartItem(userId, catId);

        return getCartItemResult.Match(cartItem => Ok(cartItem.Adapt<CartItemResponse>()), Problem);
    }

    /// <summary>Получить список всех товаров в корзине пользователя</summary>
    /// <response code="200">Список товаров из корзины пользователя</response>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(List<CartItemResponse>), 200)]
    public async Task<IActionResult> GetCartItems()
    {
        var userId = GetUserGuid();

        var cartItems = await cartService.GetCartItems(userId);

        return Ok(cartItems.Adapt<List<CartItemResponse>>());
    }

    /// <summary>Получить количество всех товаров в корзине пользователя</summary>
    /// <response code="200">Количество всех товаров в корзине пользователя</response>
    [HttpGet("count"), Authorize]
    [ProducesResponseType(typeof(TotalNumberOfCartItemsResponse), 200)]
    public async Task<IActionResult> GetCartItemsCount()
    {
        var userId = GetUserGuid();

        ErrorOr<int> getCartItemsCountResult = await cartService.GetUserCartItemsCount(userId);

        return getCartItemsCountResult.Match(count => Ok(new TotalNumberOfCartItemsResponse(count)), Problem);
    }

    /// <summary>Изменить количество штук товара из корзины пользователя</summary>
    /// <response code="204">Количество товара успешно обновлено</response>
    /// <response code="400">У товара указано количество меньше единицы</response>
    /// <response code="404">Not found</response>
    [HttpPatch("update-quantity"), Authorize]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateCartItemQuantity([Required] UpdateCartItemQuantityRequest request)
    {
        var userId = GetUserGuid();
        
        ErrorOr<Updated> updateCartItemQuantityResult = await cartService.UpdateQuantity(userId, request.CatId, request.Quantity);

        return updateCartItemQuantityResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Удалить товар из корзины пользователя</summary>
    /// <param name="catId">Guid кота, которого нужно удалить из корзины</param>
    /// <response code="204">Товар удален успешно</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{catId:guid}"), Authorize]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCartItem(Guid catId)
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> deleteCartItemResult = await cartService.DeleteCartItem(userId, catId);

        return deleteCartItemResult.Match(_ => NoContent(), Problem);
    }

    private static ErrorOr<CartItem> CreateCartItemFrom(CreateCartItemRequest request, Guid userId)
    {
        return CartItem.Create(
            userId,
            request.CatId,
            request.Quantity);
    }
}