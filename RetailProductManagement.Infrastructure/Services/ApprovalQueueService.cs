using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Core.Contracts.Services;
using RetailProductManagement.Core.Entities;
using RetailProductManagement.Infrastructure.Repositories;

namespace RetailProductManagement.Infrastructure.Services;

public class ApprovalQueueService : IApprovalQueueService
{
    private readonly IApprovalQueueRepository approvalQueueRepository;
    private readonly IProductRepository productRepository;

    public ApprovalQueueService(IApprovalQueueRepository approvalQueueRepository, IProductRepository productRepository)
    {
        this.approvalQueueRepository = approvalQueueRepository;
        this.productRepository = productRepository;
    }

    public async Task PushToApprovalQueue(Product product, string requestType, string reason)
    {
        var approvalQueueItem = new ApprovalQueue
        {
            ProductId = product.Id,
            RequestReason = reason,
            RequestType = requestType,
            RequestDate = DateTime.Now,
        };
        await approvalQueueRepository.AddAsync(approvalQueueItem);
    }

    public async Task<List<ApprovalQueue>> GetApprovalQueue()
    {
        var approvals= await approvalQueueRepository.GetAllAsync();
         return approvals.OrderBy(a => a.RequestDate).ToList();
    }

    public async Task ApproveProduct(int id)
    {
        var approvalItem = await approvalQueueRepository.GetByIdAsync(id);
        if (approvalItem == null) throw new KeyNotFoundException("Approval request not found");

        var product = await productRepository.GetByIdAsync(approvalItem.ProductId);
        if (product == null) throw new KeyNotFoundException("Product not found");

        // Process the state transition for approval
        product.ProductStatus = "Active";  // Example: Product becomes active after approval
        await productRepository.UpdateAsync(product); 
        await approvalQueueRepository.DeleteAsync(id); // Remove from queue after approval
       
    }

    public async Task RejectProduct(int id)
    {
        var approvalItem = await approvalQueueRepository.GetByIdAsync(id);
        if (approvalItem == null) throw new KeyNotFoundException("Approval request not found");

        var product = await productRepository.GetByIdAsync(approvalItem.ProductId);
        if (product == null) throw new KeyNotFoundException("Product not found");

        // Update product status to 'Rejected'
        product.ProductStatus = "Rejected";
        
        await productRepository.UpdateAsync(product);  

        // Delete approval request from the approval queue
        await approvalQueueRepository.DeleteAsync(id);
    }
}