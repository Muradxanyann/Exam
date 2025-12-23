using BankSystem.Application.Common;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController: ControllerBase
{
    private readonly ICustomerService  _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _customerService.GetAllCustomersAsync(ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(result.Error!.Message);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _customerService.GetCustomerByIdAsync(id, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error!.Message);
    }
    
    [HttpGet("email")]
    public async Task<IActionResult> GetById(string email, CancellationToken ct)
    {
        var result = await _customerService.GetByEmailAsync(email, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error!.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerCreateDto dto, CancellationToken ct)
    {
        var result = await _customerService.CreateCustomerAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value })
            : result.Error!.Type switch
            {
                ErrorType.Validation => BadRequest(result.Error!.Message),
                _ => Problem(result.Error!.Message)
            };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CustomerUpdateDto dto, CancellationToken ct)
    {
        var result = await _customerService.UpdateCustomerAsync(id, dto, ct);
        return result.IsSuccess
            ? NoContent()
            : result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error!.Message),
                ErrorType.Infrastructure => Problem("Infrastructure error",result.Error!.Message, StatusCodes.Status500InternalServerError)
            };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _customerService.DeleteCustomerAsync(id, ct);
        return result.IsSuccess
            ? NoContent()
            : result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error!.Message),
                ErrorType.Infrastructure => BadRequest(result.Error!.Message)
            };
    }
    
    [HttpPost]
    [Route("deposit")]
    public async Task<IActionResult> Deposit(int customerId, decimal amount,  CancellationToken ct)
    {
        var result = await _customerService.DepositAsync(customerId, amount, ct );
        return result.IsSuccess
            ? NoContent()
            : result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error!.Message),
                ErrorType.Infrastructure => Problem("Infrastructure error", result.Error!.Message,
                    StatusCodes.Status500InternalServerError),
                _ => BadRequest(result.Error!.Message)
            };
    }
    
    [HttpPost]
    [Route("withdraw")]
    public async Task<IActionResult> Withdraw(int customerId, decimal amount,  CancellationToken ct)
    {
        var result = await _customerService.WithdrawAsync(customerId, amount, ct );
        return result.IsSuccess
            ? NoContent()
            : result.Error!.Type switch
            {
                ErrorType.NotFound => NotFound(result.Error!.Message),
                ErrorType.Infrastructure => Problem("Infrastructure error", result.Error!.Message,
                    StatusCodes.Status500InternalServerError),
                _ => BadRequest(result.Error!.Message)
            };
    }
    
    
    
    
    
}