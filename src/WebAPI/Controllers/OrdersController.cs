namespace WebAPI.Controllers;

public class OrdersController : ApiController
{
    private readonly OrderService _orderService;
    private readonly CartService _cartService;
    private readonly CatService _catService;
    
    public OrdersController(OrderService orderService, CartService cartService, CatService catService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _catService = catService;
    }
    
    [HttpPost, Authorize]
    public IActionResult CreateOrder()
    {
        var userId = GetUserGuid();
        
        var cartItems = _cartService.GetAllUserCartItems(userId).Value.ToList();
        
        var orderItems = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            ErrorOr<Cat> getCatResult = _catService.GetCat(item.CatId);
            
            if (getCatResult.IsError)
                return Problem(getCatResult.Errors);
            
            var cat = getCatResult.Value;
            
            var orderItem = OrderItem.Create(
                catId: cat.CatId ,
                quantity: item.Quantity,
                totalPrice: cat.Cost * item.Quantity);
            
            orderItems.Add(orderItem.Value);
        }
        
        decimal totalPrice = 0;
        orderItems.ForEach(item => totalPrice += item.TotalPrice);
        
        ErrorOr<Order> orderItemsToOrderResult = CreateOrderFrom(userId, orderItems, totalPrice);
        
        if (orderItemsToOrderResult.IsError)
            return Problem(orderItemsToOrderResult.Errors);
        
        var order = orderItemsToOrderResult.Value;
        ErrorOr<Created> createOrderResult = _orderService.CreateOrder(order, cartItems);
        
        return createOrderResult.Match(_ => NoContent(), Problem);
    }
    
    [HttpGet, Authorize]
    public IActionResult GetOrders()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{orderId:guid}"), Authorize]
    public IActionResult GetOrderDetails()
    {
        throw new NotImplementedException();
    }
    
    private static ErrorOr<Order> CreateOrderFrom(Guid userId, List<OrderItem> orderItems, decimal totalPrice)
    {
        return Order.Create(
            userId,
            orderItems,
            totalPrice);
    }
    
    private Guid GetUserGuid() => Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}