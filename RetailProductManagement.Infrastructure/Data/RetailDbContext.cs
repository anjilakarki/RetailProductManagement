

using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Entities;

namespace RetailProductManagement.Infrastructure.Data;

public class RetailDbContext: DbContext
{
    public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ApprovalQueue> ApprovalQueues { get; set; }
}