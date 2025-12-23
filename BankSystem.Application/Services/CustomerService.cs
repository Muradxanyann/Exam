using System.Data.Common;
using BankSystem.Application.Common;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Exceptions;
using BankSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BankSystem.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;
    private readonly IPasswordHasher _hasher;

    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger,
        IPasswordHasher hasher)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _hasher = hasher;
    }


    public async Task<Result<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync(CancellationToken ct = default)
    {
        var entities = await _customerRepository.GetAllAsync(ct);
        var response = entities.Select(e => new CustomerResponseDto
        {
            Id = e.Id,
            Name = e.Name,
            Email = e.Email,
            IsActive = e.IsActive,
            Balance = e.Balance,
        });
        return Result<IEnumerable<CustomerResponseDto>>.Success(response);
    }

    public async Task<Result<CustomerResponseDto>> GetCustomerByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _customerRepository.GetByIdAsync(id, ct);
        if (entity == null)
            return Result<CustomerResponseDto>.Fail(new Error(ErrorType.NotFound,$"Customer with id {id} not found"));
        var response = new CustomerResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            IsActive = entity.IsActive,
            Balance = entity.Balance,
        };
        return Result<CustomerResponseDto>.Success(response);
    }

    public async Task<Result<int>> CreateCustomerAsync(CustomerCreateDto dto, CancellationToken ct = default)
    {
        var entity = await _customerRepository.GetByEmailAsync(dto.Email, ct);
        if (entity != null)
            return Result<int>.Fail(new Error(ErrorType.Conflict, "Customer with this email already exists"));
        
        try
        {
            var (hash, salt) = _hasher.Hash(dto.Password);
            var userEntity = new CustomerEntity(dto.Name, dto.Email, dto.PhoneNumber, hash, salt);
            var userId = await _customerRepository.InsertAsync(userEntity, ct);
            return Result<int>.Success(userId);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,  ex.Message);
            return Result<int>.Fail(new Error(ErrorType.Validation,ex.Message));
        }
    }

    public async  Task<Result> UpdateCustomerAsync(int id, CustomerUpdateDto dto, CancellationToken ct = default)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id, ct);
            if (customer == null)
                return Result.Fail(new Error(ErrorType.NotFound, $"Customer with id {id} not found"));
            
            customer.UpdateProfile(dto.Name, dto.PhoneNumber);
            
            var affected = await _customerRepository.UpdateAsync(customer, ct);
            if (affected)
                return Result.Success();
            return Result.Fail(new Error(ErrorType.Infrastructure, $"Cannot update customer with id {id}"));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,  ex.Message);
            return Result.Fail(new Error(ErrorType.Validation, ex.Message));
        }
    }

    public async Task<Result> DeleteCustomerAsync(int id, CancellationToken ct = default)
    {
        var user = await _customerRepository.GetByIdAsync(id, ct);
        if (user == null)
            return Result.Fail(new Error(ErrorType.NotFound,$"Customer with id {id} not found"));
        
        var affected = await _customerRepository.DeleteAsync(id, ct);
        if (affected)
            return Result.Success();
        
        return Result.Fail(new Error(ErrorType.Infrastructure,$"Cannot delete customer with id {id}"));
    }

    public async Task<Result<CustomerResponseDto>> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var entity = await _customerRepository.GetByEmailAsync(email, ct);
        if (entity == null)
            return Result<CustomerResponseDto>.Fail(new Error(ErrorType.NotFound,$"Customer with email {email} not found"));
        var response = new CustomerResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            IsActive = entity.IsActive,
        };
        return Result<CustomerResponseDto>.Success(response);
    }

    public async Task<Result> DepositAsync(int customerId, decimal amount, CancellationToken ct = default)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, ct);
            if (customer == null)
                return Result.Fail(new Error(ErrorType.NotFound, $"Customer with id {customerId} not found"));

            if (amount <= 0)
                return Result.Fail(new Error(ErrorType.InvalidOperation, $"Balance amount must be greater than 0"));
            
            customer.Deposit(amount);
            var updated = await _customerRepository.UpdateBalanceAsync(customerId, customer.Balance, ct);
            if (!updated)
                return Result.Fail(new Error(ErrorType.Infrastructure,
                    $"Cannot update customer balance with id {customerId}"));

            
            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,  ex.Message);
            return Result.Fail(new Error(ErrorType.Validation, ex.Message));
        }
        catch (DbException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return Result.Fail(new Error(ErrorType.Infrastructure,
                $"Cannot update customer balance with id {customerId}"));
        }
    }

    public async Task<Result> WithdrawAsync(int customerId, decimal amount, CancellationToken ct = default)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(customerId, ct);
            if (customer == null)
                return Result.Fail(new Error(ErrorType.NotFound, $"Customer with id {customerId} not found"));

            if (amount <= 0)
                return Result.Fail(new Error(ErrorType.InvalidOperation, $"Balance amount must be greater than 0"));

            if (customer.Balance < amount)
                return Result.Fail(new Error(ErrorType.InvalidOperation, $"Insufficient balance"));
            
            customer.Withdraw(amount);
            var updated = await _customerRepository.UpdateBalanceAsync(customerId, customer.Balance, ct);
            if (!updated)
                return Result.Fail(new Error(ErrorType.Infrastructure,
                    $"Cannot update customer balance with id {customerId}"));

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex,  ex.Message);
            return Result.Fail(new Error(ErrorType.Validation, ex.Message));
        }
        catch (DbException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return Result.Fail(new Error(ErrorType.Infrastructure,
                $"Cannot update customer balance with id {customerId}"));
        }
    }
}