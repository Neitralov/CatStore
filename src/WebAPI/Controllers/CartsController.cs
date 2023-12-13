namespace WebAPI.Controllers;

[Route("/api/cart-items")]
public class CartsController : ApiController
{
    private readonly CartService _cartService;
    
    public CartsController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost, Authorize]
    public IActionResult CreateCartItem(CreateCartItemRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> requestToCartItemResult = CreateCartItemFrom(request, userId);

        if (requestToCartItemResult.IsError)
            return Problem(requestToCartItemResult.Errors);

        var cartItem = requestToCartItemResult.Value;
        ErrorOr<Created> createCartItemResult = _cartService.StoreCartItem(cartItem);

        return createCartItemResult.Match(_ => NoContent(), Problem);
    }

    [HttpGet, Authorize]
    public IActionResult GetAllUsersCartItems()
    {
        var userId = GetUserGuid();

        ErrorOr<IEnumerable<CartItem>> getAllUserCartItemsResult = _cartService.GetAllUserCartItems(userId);
        
        return getAllUserCartItemsResult.Match(cartItems => Ok(new { CartItems = cartItems.Select(MapCartItemResponse) }), Problem);
    }

    [HttpDelete("{catId:guid}"), Authorize]
    public IActionResult DeleteCartItem(Guid catId)
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> deleteCartItemResult = _cartService.DeleteCartItem(userId, catId);

        return deleteCartItemResult.Match(_ => NoContent(), Problem);
    }

    [HttpPatch("update-quantity"), Authorize]
    public IActionResult UpdateCartItemQuantity(UpdateCartItemQuantityRequest request)
    {
        var userId = GetUserGuid();

        ErrorOr<CartItem> requestToCartItemResult = CreateCartItemFrom(request, userId);

        if (requestToCartItemResult.IsError)
            return Problem(requestToCartItemResult.Errors);

        var cartItem = requestToCartItemResult.Value;
        ErrorOr<Updated> updateCartItemQuantityResult = _cartService.UpdateQuantity(cartItem);

        return updateCartItemQuantityResult.Match(_ => NoContent(), Problem);
    }

    [HttpGet("count"), Authorize]
    public IActionResult GetCartItemsCount()
    {
        var userId = GetUserGuid();

        ErrorOr<int> getCartItemsCountResult = _cartService.GetUserCartItemsCount(userId);

        return getCartItemsCountResult.Match(count => Ok(new TotalNumberOfCartItemsResponse(count)), Problem);
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