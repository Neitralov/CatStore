namespace WebAPI.Contracts.Cat;

public record UpdateCatRequest(
    string Name,
    string SkinColor,
    string EyeColor,
    bool IsMale,
    decimal Cost)
{
    /// <example>Барсик</example>
    public string Name { get; init; } = Name;
    /// <example>#000000</example>
    public string SkinColor { get; init; } = SkinColor;
    /// <example>#ffffff</example>
    public string EyeColor { get; init; } = EyeColor;
    /// <example>true</example>
    public bool IsMale { get; init; } = IsMale;
    /// <example>100</example>
    public decimal Cost { get; init; } = Cost;
}