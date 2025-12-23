using BankSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController :  ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int customerId, CancellationToken cancellationToken = default)
    {
        var result = await _historyService.GetHistoryByCustomerIdAsync(customerId, cancellationToken);
        return result.IsSuccess 
            ? Ok(result)
            : NotFound(result.Error!.Message);
    }
}