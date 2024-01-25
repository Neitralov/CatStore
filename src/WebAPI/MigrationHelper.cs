namespace WebAPI;

/// <summary>Class containing an extension method for performing database migrations</summary>
public static class MigrationHelper
{
    /// <summary>Apply migration when web application starts</summary>
    public static async Task MigrateDatabaseAsync(this IHost webHost)
    {
        await using var scope = webHost.Services.CreateAsyncScope();
        
        var services = scope.ServiceProvider;

        await using var context = services.GetRequiredService<DatabaseContext>();
        
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "An error occurred while migrating the database.");
            throw;
        }
    }
}