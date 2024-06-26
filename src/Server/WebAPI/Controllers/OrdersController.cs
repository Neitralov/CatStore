namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/orders"), Tags("Orders")]
public class OrdersController(OrderService orderService) : ApiController
{
    /// <summary>Оформить заказ пользователя</summary>
    /// <remarks>Заказ будет сформирован на основе товаров из корзины. После этого действия корзина будет очищена.</remarks>
    /// <response code="201">Заказ успешно сформирован</response>
    /// <response code="400">Заказ не содержит товаров, стоимость заказа некорректна</response>
    /// <response code="404">Не удается найти товар, чтобы сформировать заказ</response>
    [HttpPost, Authorize]
    [ProducesResponseType(typeof(OrderResponse), 201)]
    public async Task<IActionResult> CreateOrder()
    {
        var userId = GetUserGuid();
        
        ErrorOr<List<OrderItem>> cartItemsToOrderItemsResult = await orderService.CreateOrderItemsFromCart(userId);

        if (cartItemsToOrderItemsResult.IsError)
            return Problem(cartItemsToOrderItemsResult.Errors);
        
        var orderItems = cartItemsToOrderItemsResult.Value;
        ErrorOr<Order> orderItemsToOrderResult = CreateOrderFrom(userId, orderItems);
        
        if (orderItemsToOrderResult.IsError)
            return Problem(orderItemsToOrderResult.Errors);
        
        var order = orderItemsToOrderResult.Value;
        ErrorOr<Created> createOrderResult = await orderService.StoreOrder(order);
        
        return createOrderResult.Match(_ => CreatedAtGetOrderDetails(order), Problem);
    }
    
    /// <summary>Получить конкретный заказ пользователя</summary>
    /// <param name="orderId">Guid заказа, по которому нужно получить отчет</param>
    /// <response code="200">Отчет по заказу</response>
    /// <response code="404">Not found</response>
    [HttpGet("{orderId:guid}"), Authorize]
    [ProducesResponseType(typeof(OrderResponse), 200)]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var userId = GetUserGuid();

        ErrorOr<Order> getOrderResult = await orderService.GetOrder(orderId, userId);
        
        return getOrderResult.Match(order => Ok(order.Adapt<OrderResponse>()), Problem);
    }
    
    /// <summary>Получить список заказов пользователя</summary>
    /// <response code="200">Список заказов</response>
    [HttpGet, Authorize]
    [ProducesResponseType(typeof(List<OrderResponse>), 200)]
    public async Task<IActionResult> GetOrders()
    {
        var userId = GetUserGuid();

        var orders = await orderService.GetOrders(userId);
        
        return Ok(orders.Adapt<List<OrderResponse>>());
    }

    private static ErrorOr<Order> CreateOrderFrom(
        Guid userId,
        ICollection<OrderItem> orderItems)
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
            value:       order.Adapt<OrderResponse>());
    }
}