using System.Collections;
using BankSystem.Application.Common;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.DTOs;
using BankSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BankSystem.Application.Services;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;
    private readonly ICustomerRepository _customerRepository;
    public ILogger<HistoryService> _logger;

    public HistoryService(IHistoryRepository historyRepository, ICustomerRepository customerRepository,
        ILogger<HistoryService> logger)
    {
        _historyRepository = historyRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }


    public async Task<Result<IEnumerable<HistoryResponseDto>>> GetHistoryByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            _logger.LogError("Customer with id {CustomerId} was not found", customerId);
            return Result<IEnumerable<HistoryResponseDto>>.Fail(new Error(ErrorType.NotFound, $"Customer {customerId} not found"));
        }
        
        var history = await _historyRepository.GetHistoryByCustomerIdAsync(customerId,  cancellationToken);

        var response = history.Select(h => new HistoryResponseDto
        {
            Date = h.Date,
            Amount = h.Amount,
            OperationType = h.OperationType,
            Status = h.Status,
            SenderCustomerId = h.SenderCustomerId,
            ReceiverCustomerId = h.ReceiverCustomerId
        });
         return Result<IEnumerable<HistoryResponseDto>>.Success(response);
    }
}