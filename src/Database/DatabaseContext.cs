namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Cat> Cats => Set<Cat>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) => Database.EnsureCreated();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>()
            .HasKey(entity => new { entity.UserId, entity.CatId });

        modelBuilder.Entity<OrderItem>()
            .HasKey(entity => new { entity.OrderId, entity.CatId });

        var admin = User.Create(
            email: "admin@gmail.com",
            password: "admin",
            confirmPassword: "admin",
            role: "admin")
            .Value;

        modelBuilder.Entity<User>().HasData(admin);
    }
}