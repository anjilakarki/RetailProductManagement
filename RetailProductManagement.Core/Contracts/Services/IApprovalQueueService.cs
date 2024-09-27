using RetailProductManagement.Core.Entities;

namespace RetailProductManagement.Core.Contracts.Services;

public interface IApprovalQueueService
{
    Task PushToApprovalQueue(Product product, string requestType, string reason);
    Task ApproveProduct(int id);
    Task<List<ApprovalQueue>> GetApprovalQueue();
    Task RejectProduct(int id);
}