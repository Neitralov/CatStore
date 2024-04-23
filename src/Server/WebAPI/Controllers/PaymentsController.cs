namespace WebAPI.Controllers;

/// <inheritdoc />
[Route("/api/payments"), Tags("Payments")]
public class PaymentsController(IHttpClientFactory factory, OrderService orderService) : ApiController
{
    /// <summary>Оформить платеж по заказу</summary>
    /// <response code="200">Платеж успешно создан</response>
    /// <response code="404">Not found</response>
    [HttpPost, Authorize]
    [ProducesResponseType(typeof(PaymentResponse), 200)]
    public async Task<IActionResult> CreatePayment([Required] OrderResponse orderData)
    {
        var userId = GetUserGuid();
        ErrorOr<Order> getOrderResult = await orderService.GetOrder(orderData.OrderId, userId);

        if (getOrderResult.IsError)
            return Problem(getOrderResult.Errors);
        
        using var request = new HttpRequestMessage(HttpMethod.Post, "/v3/payments");
        request.Headers.Add("Idempotence-Key", $"{orderData.OrderId}");
        
        request.Content = JsonContent.Create(new CreatePaymentRequest(
            new Amount(orderData.TotalPrice),
            new RequestConfirmation("http://localhost:8080"),
            Description: $"Платеж по заказу {orderData.OrderId}"));

        var httpClient = factory.CreateClient("kassa"); 
        
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode is false)
            return BadRequest();
        
        var result = await response.Content.ReadFromJsonAsync<PaymentResponse>();
        
        return Ok(result);
    }
}