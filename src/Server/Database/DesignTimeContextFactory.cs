namespace Database;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private const string ConnectionString = "Data Source=../WebAPI/CatStore/Database.db";
    
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlite(ConnectionString);
        
        return new DatabaseContext(optionsBuilder.Options);
    }
}