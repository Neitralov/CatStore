namespace WebAPI.Controllers;

/// <inheritdoc />
public class OrdersController(
    OrderService orderService,
    CartService cartService,
    CatService catService) : ApiController
{
    /// <summary>Оформить заказ пользователя</summary>
    /// <remarks>Заказ будет сформирован на основе товаров из корзины. После этого действия корзина будет очищена.</remarks>
    /// <response code="201">Заказ успешно сформирован</response>
    /// <response code="400">Заказ не содержит товаров, стоимость заказа некорректна</response>
    /// <response code="404">Не удается найти товар, чтобы сформировать заказ</response>
    [HttpPost, Authorize]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    public IActionResult CreateOrder()
    {
        var userId = GetUserGuid();
        
        var cartItems = cartService.GetCartItems(userId).Value.ToList();
        
        var orderItems = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            ErrorOr<Cat> getCatResult = catService.GetCat(item.CatId);
            
            if (getCatResult.IsError)
                return Problem(getCatResult.Errors);
            
            var cat = getCatResult.Value;
            
            var orderItem = OrderItem.Create(
                catId:      cat.CatId,
                catName:    cat.Name,
                quantity:   item.Quantity,
                totalPrice: (cat.Cost - cat.Discount) * item.Quantity);
            
            if (orderItem.IsError)
                return Problem(orderItem.Errors);
            
            orderItems.Add(orderItem.Value);
        }
        
        ErrorOr<Order> orderItemsToOrderResult = CreateOrderFrom(userId, orderItems);
        
        if (orderItemsToOrderResult.IsError)
            return Problem(orderItemsToOrderResult.Errors);
        
        var order = orderItemsToOrderResult.Value;
        ErrorOr<Created> createOrderResult = orderService.StoreOrder(order, cartItems);
        
        return createOrderResult.Match(_ => CreatedAtGetOrderDetails(order), Problem);
    }
    
    /// <summary>Получить конкретный заказ пользователя</summary>
    /// <param name="orderId">Guid заказа, по которому нужно получить отчет</param>
    /// <response code="200">Отчет по заказу</response>
    /// <response code="404">Not found</response>
    [HttpGet("{orderId:guid}"), Authorize]
    [ProducesResponseType(typeof(OrderResponse), 200)]
    public IActionResult GetOrder(Guid orderId)
    {
        var userId = GetUserGuid();

        ErrorOr<Order> getOrderResult = orderService.GetOrder(orderId, userId);

        return getOrderResult.Match(order => Ok(MapOrderResponse(order)), Problem);
    }
    
    /// <summary>Получить список заказов пользователя</summary>
    /// <response code="200">Список заказов</response>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(List<OrderResponse>), 200)]
    public IActionResult GetOrders()
    {
        var userId = GetUserGuid();

        ErrorOr<IEnumerable<Order>> getOrdersResult = orderService.GetOrders(userId);

        return getOrdersResult.Match(orders => Ok(new List<OrderResponse>(orders.Select(MapOrderResponse))), Problem);
    }

    private static ErrorOr<Order> CreateOrderFrom(
        Guid userId,
        List<OrderItem> orderItems)
    {
        return Order.Create(
            userId,
            orderItems);
    }
    
    private CreatedAtActionResult CreatedAtGetOrderDetails(Order order)
    {
        return CreatedAtAction(
            actionName:  nameof(GetOrder),
            routeValues: new { orderId = order.OrderId },
            value:       MapOrderResponse(order));
    }

    private static OrderResponse MapOrderResponse(Order order)
    {
        var orderCatResponses = order.OrderItems.Select(MapOrderCatResponse);

        return new OrderResponse(
            order.OrderId,
            order.OrderDate,
            order.TotalPrice,
            orderCatResponses);
    }

    private static OrderCatResponse MapOrderCatResponse(OrderItem orderItem)
    {
        return new OrderCatResponse(
            orderItem.CatId,
            orderItem.Name,
            orderItem.Quantity,
            orderItem.TotalPrice);
    }
}