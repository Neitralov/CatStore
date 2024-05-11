namespace WebAPI;

/// <summary>Class containing an extension method for database operations</summary>
public static class SeedData
{
    /// <summary>Adds data the first time the application is launched</summary>
    public static async Task SeedDataAsync(this IHost webHost)
    {
        await using var scope = webHost.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var database = services.GetRequiredService<IMongoDatabase>();

        using var cursor = await database.ListCollectionsAsync();
        var isFirstLaunch = cursor.ToList().Count != 0;
        
        if (isFirstLaunch)
            return;
        
        var admin 
            = User.Create(
                    email: "admin@gmail.com",
                    password: "admin",
                    confirmPassword: "admin",
                    canEditCats: true)
                .Value;
        
        var collection = database.GetCollection<User>("users");
        await collection.InsertOneAsync(admin);
    }
}