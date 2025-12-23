using BankSystem.Domain.Enums;

namespace BankSystem.Domain.DTOs;

public class HistoryResponseDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public OperationType? OperationType {get; set; }
    public OperationStatus? Status { get; set; }
    public int? SenderCustomerId { get; set; }
    public int? ReceiverCustomerId { get; set; }
}