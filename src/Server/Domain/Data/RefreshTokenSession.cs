namespace Domain.Data;

public class RefreshTokenSession
{
    [BsonId]
    public string Token { get; private set; } = default!;
    public Guid UserId { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    
    public const int ExpiresInDays = 30;
    public const int MaxRefreshTokenSessionsPerUser = 5;
    
    private RefreshTokenSession() { }
    
    public static RefreshTokenSession Create(
        Guid userId)
    {
        var refreshToken = Guid.NewGuid().ToString();
        
        return new RefreshTokenSession
        {
            UserId = userId,
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(ExpiresInDays)
        };
    }
    
    public void Update()
    {
        Token = Guid.NewGuid().ToString();
        ExpirationDate = DateTime.UtcNow.AddDays(ExpiresInDays);
    }
}