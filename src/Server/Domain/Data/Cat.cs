namespace Domain.Data;

public class Cat
{
    public Guid CatId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string SkinColor { get; private set; } = string.Empty;
    public string EyeColor { get; private set; } = string.Empty;
    public bool IsMale { get; private set; }
    public decimal Cost { get; private set; }
    
    public const int MinNameLength = 3;
    public const int MaxNameLength = 15;
    
    private Cat() { }
    
    public static ErrorOr<Cat> Create(
        string name,
        string skinColor,
        string eyeColor,
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
           IsMale    = isMale,
           Cost      = cost
       };
    }

    public void UpdateCat(Cat cat)
    {
        Name      = cat.Name;
        SkinColor = cat.SkinColor;
        EyeColor  = cat.EyeColor;
        IsMale    = cat.IsMale;
        Cost      = cat.Cost; 
    }
}