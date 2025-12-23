using BankSystem.Domain.Exceptions;

namespace BankSystem.Domain.Entities;

public class CustomerEntity :  BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }
    public decimal Balance { get; private set; }
    public bool IsActive { get; private set; }

    public CustomerEntity(string name, string email, string phoneNumber, string passwordHash, string passwordSalt)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidatePassword(passwordHash, passwordSalt);
        ValidatePhone(phoneNumber);
        
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Balance = 0;
        IsActive = true;
    }
    
    public static CustomerEntity Restore(
        int id,
        string name,
        string email,
        string phoneNumber,
        string passwordHash,
        string passwordSalt,
        decimal balance,
        bool isActive)
    {
        ValidateBalance(balance);
        var customer = new CustomerEntity(name, email, phoneNumber, passwordHash, passwordSalt)
        {
            Id = id,
            Balance = balance,
            IsActive = isActive
        };
        return customer;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be empty");
        
        if (name.Length < 2)
            throw new DomainException("Name cannot be less than 2");
    }
    
    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new DomainException("Email cannot be empty");
        
        if (!email.Contains('@'))
            throw new DomainException("Invalid email address");
    }
    
    private static void ValidatePassword(string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException("Password hash is required");

        if (string.IsNullOrWhiteSpace(salt))
            throw new DomainException("Password salt is required");
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone number is required");
    }

    private static void ValidateBalance(decimal balance)
    {
        if (balance < 0)
            throw new DomainException("Balance cannot be negative");
    }
    public void UpdateProfile(string name, string phone)
    {
        ValidateName(name);
        ValidatePhone(phone);

        Name = name;
        PhoneNumber = phone;
    }

    public void Deposit(decimal balance)
    {
        Balance += balance;
    }

    public void Withdraw(decimal balance)
    {
        Balance -= balance;
        ValidateBalance(balance);
    }
}

