namespace Domain.Data;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public DateTime DateCreated { get; set; }
    public string Role { get; set; } = "customer";
}