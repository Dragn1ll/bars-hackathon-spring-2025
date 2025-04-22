namespace Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    public IUserRepository Users { get; set; }
    //todo добавить остальные репозитории
    public Task SaveChangesAsync();
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
}