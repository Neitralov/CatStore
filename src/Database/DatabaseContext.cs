namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Cat> Cats => Set<Cat>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) => Database.EnsureCreated();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasKey(entity => new { entity.CatId, entity.Name });

        modelBuilder.Entity<CartItem>()
            .HasKey(entity => new { entity.UserId, entity.CatId });

        var admin = User.Create(
            email: "admin@gmail.com",
            password: "admin",
            confirmPassword: "admin",
            role: "admin")
            .Value;

        modelBuilder.Entity<User>().HasData(admin);
    }
}