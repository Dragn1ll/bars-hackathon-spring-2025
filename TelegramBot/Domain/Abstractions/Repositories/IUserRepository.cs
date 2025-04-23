using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IUserRepository: IRepository<UserEntity>
{
    Task<bool> PatchUsernameAsync(int userId, string newUsername);
    Task<bool> PatchUserEmailAsync(int userId, string newEmail);
    Task<bool> PatchUserPhoneAsync(int userId, string newPhone);
    Task<bool> PatchUserDeleteStatusAsync(int userId);
    Task<IEnumerable<UserEntity?>> GetAllUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(int userId);
    Task<UserEntity?> GetUserByUsernameAsync(string username);
    Task<UserEntity?> GetUserByEmailAsync(string email);
    Task<UserEntity?> GetUserByPhoneAsync(string phone);
    Task<IEnumerable<UserEntity?>> GetDeletedUsersAsync();
    Task<bool> DeleteUserByIdAsync(int userId);
    Task<bool> DeleteUserByUsernameAsync(string username);
    Task<bool> DeleteUserByEmailAsync(string email);
    Task<bool> DeleteUserByPhoneAsync(string phone);
}