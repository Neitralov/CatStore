namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Cat> Cats => Set<Cat>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) => Database.EnsureCreated();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>()
            .HasKey(entity => new { entity.UserId, entity.CatId });
    }
}