using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Repositories;

public interface ICustomerRepository : IRepository<CustomerEntity>
{
    public Task<CustomerEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<bool> UpdateBalanceAsync(int customerId, decimal balance, CancellationToken cancellationToken = default);
}