using BankSystem.Domain.Enums;

namespace BankSystem.Domain.Entities;

public class HistoryEntity : BaseEntity
{
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public OperationType? OperationType {get; private set; }
    public OperationStatus? Status { get; private set; }
    public int? SenderCustomerId { get; private set; }
    public int? ReceiverCustomerId { get; private set; }
}


