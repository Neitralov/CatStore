namespace Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Cat> Cats => Set<Cat>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<RefreshTokenSession> RefreshTokenSessions => Set<RefreshTokenSession>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>()
            .HasKey(entity => new { entity.UserId, entity.CatId });

        modelBuilder.Entity<OrderItem>()
            .HasKey(entity => new { entity.OrderId, entity.CatId });

        modelBuilder.Entity<RefreshTokenSession>()
            .HasKey(entity => entity.SessionId);

        var admin = User.Create(
            email: "admin@gmail.com",
            password: "admin",
            confirmPassword: "admin",
            canEditCats: true)
            .Value;

        modelBuilder.Entity<User>().HasData(admin);
    }
}