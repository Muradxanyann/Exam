namespace BankSystem.Application.Common;

public class Error
{
    public ErrorType Type { get; set; }
    public string Message { get; set; }

    public Error(ErrorType type, string message)
    {
        Type = type;
        Message = message;
    }
        
}