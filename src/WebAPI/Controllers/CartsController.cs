namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/cart-items"), Tags("CartItems")]
public class CartsController : ApiController
{
    private readonly CartService _cartService;
    
    /// <inheritdoc />
    public CartsController(CartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>Добавить товар в корзину пользователя</summary>
    /// <response code="204">Товар успешно добавлен в корзину</response>
    /// <response code="400">У товара указано количество меньше единицы</response>
    /// <response code="404">Not found</response>
    [HttpPost, Authorize]
    [ProducesResponseType(204)]
    public IActionResult CreateCartItem([Required] CreateCartItemRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> requestToCartItemResult = CreateCartItemFrom(request, userId);

        if (requestToCartItemResult.IsError)
            return Problem(requestToCartItemResult.Errors);

        var cartItem = requestToCartItemResult.Value;
        ErrorOr<Created> createCartItemResult = _cartService.StoreCartItem(cartItem);

        return createCartItemResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Получить список всех товаров в корзине пользователя</summary>
    /// <response code="200">Список товаров из корзины пользователя</response>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(List<CartItemResponse>), 200)]
    public IActionResult GetAllUsersCartItems()
    {
        var userId = GetUserGuid();

        ErrorOr<IEnumerable<CartItem>> getAllUserCartItemsResult = _cartService.GetAllUserCartItems(userId);
        
        return getAllUserCartItemsResult.Match(cartItems => Ok(new List<CartItemResponse>(cartItems.Select(MapCartItemResponse))), Problem);
    }

    /// <summary>Получить количество всех товаров в корзине пользователя</summary>
    /// <response code="200">Количество всех товаров в корзине пользователя</response>
    [HttpGet("count"), Authorize]
    [ProducesResponseType(typeof(TotalNumberOfCartItemsResponse), 200)]
    public IActionResult GetCartItemsCount()
    {
        var userId = GetUserGuid();

        ErrorOr<int> getCartItemsCountResult = _cartService.GetUserCartItemsCount(userId);

        return getCartItemsCountResult.Match(count => Ok(new TotalNumberOfCartItemsResponse(count)), Problem);
    }

    /// <summary>Изменить количество штук товара из корзины пользователя</summary>
    /// <response code="204">Количество товара успешно обновлено</response>
    /// <response code="400">У товара указано количество меньше единицы</response>
    /// <response code="404">Not found</response>
    [HttpPatch("update-quantity"), Authorize]
    [ProducesResponseType(204)]
    public IActionResult UpdateCartItemQuantity([Required] UpdateCartItemQuantityRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> requestToCartItemResult = CreateCartItemFrom(request, userId);

        if (requestToCartItemResult.IsError)
            return Problem(requestToCartItemResult.Errors);

        var cartItem = requestToCartItemResult.Value;
        ErrorOr<Updated> updateCartItemQuantityResult = _cartService.UpdateQuantity(cartItem);

        return updateCartItemQuantityResult.Match(_ => NoContent(), Problem);
    }

    /// <summary>Удалить товар из корзины пользователя</summary>
    /// <param name="catId">Guid кота, которого нужно удалить из корзины</param>
    /// <response code="204">Товар удален успешно</response>
    /// <response code="404">Not found</response>
    [HttpDelete("{catId:guid}"), Authorize]
    [ProducesResponseType(204)]
    public IActionResult DeleteCartItem(Guid catId)
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> deleteCartItemResult = _cartService.DeleteCartItem(userId, catId);

        return deleteCartItemResult.Match(_ => NoContent(), Problem);
    }

    private static ErrorOr<CartItem> CreateCartItemFrom(CreateCartItemRequest request, Guid userId)
    {
        return CartItem.Create(
            userId,
            request.CatId);
    }

    private static ErrorOr<CartItem> CreateCartItemFrom(UpdateCartItemQuantityRequest request, Guid userId)
    {
        return CartItem.Create(
            userId,
            request.CatId,
            request.Quantity);
    }

    private static CartItemResponse MapCartItemResponse(CartItem cartItem)
    {
        return new CartItemResponse(
            cartItem.CatId,
            cartItem.Quantity);
    }
}