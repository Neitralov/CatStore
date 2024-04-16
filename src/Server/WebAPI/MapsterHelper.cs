namespace WebAPI;

/// <summary>Class containing an extension method for applying Mapster TypeAdapterConfig</summary>
public static class MapsterHelper
{
    /// <summary>Apply all TypeAdapterConfigs</summary>
    public static void ApplyTypeAdapterConfigs(this IHost _)
    {
        TypeAdapterConfig<Order, OrderResponse>
            .NewConfig()
            .Map(
                member: orderResponse => orderResponse.Cats,
                source: order => order.OrderItems.Adapt<List<OrderCatResponse>>());
    }
}