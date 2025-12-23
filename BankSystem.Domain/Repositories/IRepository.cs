using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);
    public Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}