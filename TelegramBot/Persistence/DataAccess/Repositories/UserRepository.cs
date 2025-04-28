using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : 
    AbstractRepository<UserEntity>(context), 
    IUserRepository
{
    public async Task<bool> PatchUsernameAsync(Guid userId, string newUsername)
    {
        return await PatchAsync(userId, e => e.Username = newUsername);
    }

    public async Task<bool> PatchUserEmailAsync(Guid userId, string newEmail)
    {
        return await PatchAsync(userId, e => e.Email = newEmail);
    }

    public async Task<bool> PatchUserPhoneAsync(Guid userId, string newPhone)
    {
        return await PatchAsync(userId, e => e.PhoneNumber = newPhone);
    }

    public async Task<bool> PatchUserDeleteStatusAsync(Guid userId)
    {
        return await PatchAsync(userId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<IEnumerable<UserEntity?>> GetAllUsersAsync()
    {
        return await GetAllByFilterAsync(e => true);
    }

    public async Task<UserEntity?> GetUserByIdAsync(int userId)
    {
        return await GetByFilterAsync(e => e.UserId == userId);
    }

    public async Task<UserEntity?> GetUserByUsernameAsync(string username)
    {
        return await GetByFilterAsync(e => e.Username == username);
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await GetByFilterAsync(e => e.Email == email);
    }

    public async Task<UserEntity?> GetUserByPhoneAsync(string phone)
    {
        return await GetByFilterAsync(e => e.PhoneNumber == phone);
    }

    public async Task<IEnumerable<UserEntity?>> GetDeletedUsersAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted);
    }

    public async Task<bool> DeleteUserByIdAsync(int userId)
    {
        return await DeleteAsync(e => e.UserId == userId);
    }

    public Task<bool> DeleteUserByUsernameAsync(string username)
    {
        return DeleteAsync(e => e.Username == username);
    }

    public Task<bool> DeleteUserByEmailAsync(string email)
    {
        return DeleteAsync(e => e.Email == email);
    }

    public Task<bool> DeleteUserByPhoneAsync(string phone)
    {
        return DeleteAsync(e => e.PhoneNumber == phone);
    }
}