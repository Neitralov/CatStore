namespace WebAPI.Contracts.Cat;

public record UpdateCatPriceRequest(decimal Cost)
{
    /// <example>125</example>
    public decimal Cost { get; init; } = Cost;
}