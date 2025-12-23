namespace BankSystem.Domain.DTOs;

public class CustomerDbRow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get;  set; }
}