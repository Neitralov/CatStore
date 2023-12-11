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
        var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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
        var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        ErrorOr<IEnumerable<CartItem>> getAllUserCartItemsResult = _cartService.GetAllUserCartItems(userId);
        
        return getAllUserCartItemsResult.Match(cartItems => Ok(new { CartItems = cartItems.Select(MapCartItemResponse) }), Problem);
    }

    [HttpDelete("{catId:guid}"), Authorize]
    public IActionResult RemoveCartItem(Guid catId)
    {
        var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        ErrorOr<Deleted> removedCartItemResult = _cartService.RemoveCartItem(userId, catId);

        return removedCartItemResult.Match(_ => NoContent(), Problem);
    }

    [HttpPatch("update-quantity"), Authorize]
    public IActionResult UpdateCartItemQuantity(UpdateCartItemQuantityRequest request)
    {
        throw new NotImplementedException();
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

    private static CartItemResponse MapCartItemResponse(CartItem cartItem)
    {
        return new CartItemResponse(
            cartItem.CatId,
            cartItem.Quantity);
    }
}