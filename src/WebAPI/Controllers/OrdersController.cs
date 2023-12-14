namespace WebAPI.Controllers;

/// <inheritdoc />
public class OrdersController : ApiController
{
    private readonly OrderService _orderService;
    private readonly CartService _cartService;
    private readonly CatService _catService;
    
    /// <inheritdoc />
    public OrdersController(
        OrderService orderService,
        CartService cartService,
        CatService catService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _catService = catService;
    }
    
    /// <summary>Оформить заказ пользователя</summary>
    /// <remarks>Заказ будет сформирован на основе товаров из корзины. После этого действия корзина будет очищена.</remarks>
    /// <response code="204">Заказ успешно сформирован</response>
    /// <response code="400">Заказ не содержит товаров, стоимость заказа некорректна</response>
    /// <response code="404">Не удается найти товар, чтобы сформировать заказ</response>
    [HttpPost, Authorize]
    [ProducesResponseType(204)]
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
                catId:      cat.CatId ,
                quantity:   item.Quantity,
                totalPrice: cat.Cost * item.Quantity);
            
            orderItems.Add(orderItem.Value);
        }
        
        decimal totalPrice = 0;
        orderItems.ForEach(item => totalPrice += item.TotalPrice);
        
        ErrorOr<Order> orderItemsToOrderResult = CreateOrderFrom(userId, orderItems, totalPrice);
        
        if (orderItemsToOrderResult.IsError)
            return Problem(orderItemsToOrderResult.Errors);
        
        var order = orderItemsToOrderResult.Value;
        ErrorOr<Created> createOrderResult = _orderService.StoreOrder(order, cartItems);
        
        return createOrderResult.Match(_ => NoContent(), Problem);
    }
    
    /// <summary>Получить список заказов пользователя</summary>
    /// <response code="200">Список заказов</response>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(List<OrderResponse>), 200)]
    public IActionResult GetOrders()
    {
        var userId = GetUserGuid();

        ErrorOr<IEnumerable<Order>> getAllUserOrdersResult = _orderService.GetAllUserOrders(userId);

        return getAllUserOrdersResult.Match(orders => Ok(new List<OrderResponse>(orders.Select(MapOrderResponse))), Problem);
    }
    
    /// <summary>Получить подробный отчет по заказу пользователя</summary>
    /// <param name="orderId">Guid заказа, по которому нужно получить отчет</param>
    /// <response code="200">Отчет по заказу</response>
    /// <response code="404">Not found</response>
    [HttpGet("{orderId:guid}"), Authorize]
    [ProducesResponseType(typeof(OrderDetailsResponse), 200)]
    public IActionResult GetOrderDetails(Guid orderId)
    {
        var userId = GetUserGuid();

        ErrorOr<Order> getOrderResult = _orderService.GetOrder(orderId, userId);

        return getOrderResult.Match(order => Ok(MapOrderDetailsResponse(order)), Problem);
    }
    
    private static ErrorOr<Order> CreateOrderFrom(
        Guid userId,
        List<OrderItem> orderItems,
        decimal totalPrice)
    {
        return Order.Create(
            userId,
            orderItems,
            totalPrice);
    }
    
    private static OrderResponse MapOrderResponse(Order order)
    {
        return new OrderResponse(
            order.OrderId,
            order.OrderDate,
            order.TotalPrice);
    }

    private static OrderDetailsResponse MapOrderDetailsResponse(Order order)
    {
        var orderDetailsCatResponses = order.OrderItems.Select(MapOrderDetailsCatResponse);

        return new OrderDetailsResponse(
            order.OrderId,
            order.OrderDate,
            order.TotalPrice,
            orderDetailsCatResponses);
    }

    private static OrderDetailsCatResponse MapOrderDetailsCatResponse(OrderItem orderItem)
    {
        return new OrderDetailsCatResponse(
            orderItem.CatId,
            orderItem.Quantity,
            orderItem.TotalPrice);
    }
}