using System.ComponentModel.DataAnnotations;

namespace BankSystem.Domain.DTOs;

public class CustomerCreateDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; init; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; init; } = string.Empty;
}