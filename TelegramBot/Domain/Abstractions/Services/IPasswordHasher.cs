namespace Domain.Abstractions.Services;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Validate(string password, string passwordHash);
}