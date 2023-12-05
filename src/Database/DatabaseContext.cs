namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Cat> Cats => Set<Cat>();
    public DbSet<User> Users => Set<User>();
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) => Database.EnsureCreated();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Cat>().Property(p => p.CatId).IsRequired().ValueGeneratedNever();
    }
}