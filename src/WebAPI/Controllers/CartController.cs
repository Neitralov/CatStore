namespace WebAPI.Controllers;

[Route("/api/cart-items")]
public class CartController : ApiController
{
    private readonly CartService _cartService;
    private readonly CatService _catService;
    
    public CartController(CartService cartService, CatService catService)
    {
        _cartService = cartService;
        _catService = catService;
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
    public IActionResult RemoveCartItem(Guid catId)
    {
        var userId = GetUserGuid();

        ErrorOr<Deleted> removedCartItemResult = _cartService.RemoveCartItem(userId, catId);

        return removedCartItemResult.Match(_ => NoContent(), Problem);
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
    public IActionResult GetTotalNumberOfItemsInCart()
    {
        throw new NotImplementedException();
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

    private Guid GetUserGuid() => Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}