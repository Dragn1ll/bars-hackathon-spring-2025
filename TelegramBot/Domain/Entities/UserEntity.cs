namespace Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string Email { get; set; }
    public int? Age { get; set; }
    public DateTime? JoinDate { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool IsDeleted { get; set; }
}