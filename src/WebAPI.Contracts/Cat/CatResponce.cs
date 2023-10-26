namespace WebAPI.Contracts.Cat;

public record CatResponse(
     Guid CatId,
     string Name,
     string SkinColor,
     string EyeColor,
     bool IsMale,
     decimal Cost
);