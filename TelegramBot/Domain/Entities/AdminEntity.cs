namespace Domain.Entities;

public class AdminEntity
{
    public Guid AdminId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}