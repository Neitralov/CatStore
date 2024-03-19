namespace Domain.Data;

public class Cat
{
    public Guid CatId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string SkinColor { get; private set; } = string.Empty;
    public string EyeColor { get; private set; } = string.Empty;
    public string EarColor { get; private set; } = string.Empty;
    public bool IsMale { get; private set; }
    public decimal Cost { get; private set; }
    
    public const int MinNameLength = 3;
    public const int MaxNameLength = 15;
    
    private Cat() { }
    
    public static ErrorOr<Cat> Create(
        string name,
        string skinColor,
        string eyeColor,
        string earColor,
        bool isMale,
        decimal cost,
        Guid? catId = null)
    {
       List<Error> errors = new();
       
       if (name.Length is < MinNameLength or > MaxNameLength)
           errors.Add(Errors.Cat.InvalidName);
       
       var correctHexColorPattern = new Regex(@"#[0-9a-f]{6}$", RegexOptions.Compiled);
       
       if (correctHexColorPattern.IsMatch(skinColor) is false)
           errors.Add(Errors.Cat.InvalidSkinColor);
       
       if (correctHexColorPattern.IsMatch(eyeColor) is false)
           errors.Add(Errors.Cat.InvalidEyeColor);

       if (correctHexColorPattern.IsMatch(earColor) is false)
           errors.Add(Errors.Cat.InvalidEarColor);
       
       if (cost <= 0)
           errors.Add(Errors.Cat.InvalidCost);
       
       if (errors.Count > 0)
           return errors;
       
       return new Cat
       {
           CatId     = catId ?? Guid.NewGuid(),
           Name      = name,
           SkinColor = skinColor,
           EyeColor  = eyeColor,
           EarColor  = earColor,
           IsMale    = isMale,
           Cost      = cost
       };
    }

    public ErrorOr<Updated> UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            return Errors.Cat.InvalidCost;

        Cost = newPrice;

        return Result.Updated;
    }
}