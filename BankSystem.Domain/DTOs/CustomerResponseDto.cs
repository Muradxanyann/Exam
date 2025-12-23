namespace BankSystem.Domain.DTOs;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    
    public decimal Balance { get; set; }
}