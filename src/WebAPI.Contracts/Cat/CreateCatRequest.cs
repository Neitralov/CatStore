namespace WebAPI.Contracts.Cat;

public record CreateCatRequest(
    string Name,
    string SkinColor,
    string EyeColor,
    bool IsMale,
    decimal Cost);