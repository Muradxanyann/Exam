using BankSystem.Application.Common;
using BankSystem.Domain.DTOs;

namespace BankSystem.Application.Interfaces;

public interface IHistoryService
{
    public Task<Result<IEnumerable<HistoryResponseDto>>> GetHistoryByCustomerIdAsync(int customerId,  CancellationToken cancellationToken = default);
}