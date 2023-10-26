namespace WebAPI.Contracts.Cat;

public record UpdateCatRequest(
    string Name,
    string SkinColor,
    string EyeColor,
    bool IsMale,
    decimal Cost);