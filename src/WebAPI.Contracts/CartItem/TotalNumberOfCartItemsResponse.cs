namespace WebAPI.Contracts.CartItem;

public record TotalNumberOfCartItemsResponse(
    int Count)
{
    /// <example>2</example>
    public int Count { get; init; } = Count;
}