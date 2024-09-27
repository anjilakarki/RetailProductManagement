using Microsoft.AspNetCore.Mvc;
using RetailProductManagement.Core.Contracts.Services;
using RetailProductManagement.Infrastructure.Services;

namespace RetailProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApprovalQueueController: ControllerBase
{
    private readonly IApprovalQueueService _approvalQueueService;

    public ApprovalQueueController(IApprovalQueueService approvalQueueService)
    {
        _approvalQueueService = approvalQueueService;
    }
    // Get all products in approval queue
    [HttpGet]
    public async Task<IActionResult> GetApprovalQueue()
    {
        var approvalQueue = await _approvalQueueService.GetApprovalQueue();
        return Ok(approvalQueue);
    }

    // Approve product from queue
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveProduct(int id)
    {
        try
        {
            await _approvalQueueService.ApproveProduct(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Reject product from queue
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectProduct(int id)
    {
        try
        {
            await _approvalQueueService.RejectProduct(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}