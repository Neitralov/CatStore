namespace WebAPI.Contracts.Cat;

public record UpdateCatPriceRequest(decimal Cost, decimal Discount)
{
    /// <example>125</example>
    public decimal Cost { get; init; } = Cost;
    /// <example>25</example>
    public decimal Discount { get; init; } = Discount;
}