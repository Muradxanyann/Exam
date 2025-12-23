using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Repositories;

public interface IHistoryRepository
{
    public Task<IEnumerable<HistoryEntity>> GetHistoryByCustomerIdAsync(int customerId,  CancellationToken cancellationToken);
}