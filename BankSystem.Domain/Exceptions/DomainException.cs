using BankSystem.Domain.Entities;

namespace BankSystem.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string error) :  base(error){}
}