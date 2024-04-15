namespace Database;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=catstore;Username=postgres;Password=1234";
    
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql(ConnectionString);
        
        return new DatabaseContext(optionsBuilder.Options);
    }
}