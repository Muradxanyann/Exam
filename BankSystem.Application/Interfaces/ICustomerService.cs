using BankSystem.Application.Common;
using BankSystem.Domain.DTOs;

namespace BankSystem.Application.Interfaces;

public interface ICustomerService
{
    public Task<Result<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync(CancellationToken ct = default);
    public Task<Result<CustomerResponseDto>> GetCustomerByIdAsync(int id,CancellationToken ct = default);
    public Task<Result<int>> CreateCustomerAsync(CustomerCreateDto dto,  CancellationToken ct = default);
    public Task<Result> UpdateCustomerAsync(int id, CustomerUpdateDto dto,CancellationToken ct = default);
    public Task<Result> DeleteCustomerAsync(int id, CancellationToken ct = default);
    
    public Task<Result<CustomerResponseDto>> GetByEmailAsync(string email, CancellationToken ct = default);
    
    public Task<Result> DepositAsync(int customerId, decimal amount, CancellationToken ct = default);
    public Task<Result> WithdrawAsync(int customerId, decimal amount, CancellationToken ct = default);

    
}