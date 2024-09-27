using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Core.Entities;
using RetailProductManagement.Infrastructure.Data;

namespace RetailProductManagement.Infrastructure.Repositories;

public class ApprovalQueueRepository: BaseRepository<ApprovalQueue>, IApprovalQueueRepository
{
    public ApprovalQueueRepository(RetailDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<ApprovalQueue>> GetAllAsync()
    {
        return await _dbContext.Set<ApprovalQueue>()
            .Include(a => a.Product)
            .ToListAsync();
    }
}