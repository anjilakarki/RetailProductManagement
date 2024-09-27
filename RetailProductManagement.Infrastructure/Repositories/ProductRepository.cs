using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Core.Entities;
using RetailProductManagement.Infrastructure.Data;

namespace RetailProductManagement.Infrastructure.Repositories;

public class ProductRepository: BaseRepository<Product>, IProductRepository
{
    public ProductRepository(RetailDbContext dbContext) : base(dbContext)
    {
    }

    // public async Task<List<Product>> GetActiveProducts()
    // {
    //     var products = await  _dbContext.Products.Where(p => p.ProductStatus == "Active")
    //         .OrderByDescending(p => p.CreatedDate).ToListAsync();
    //     return products;
    // }
    
    public IQueryable<Product> GetActiveProducts()
    {
        return _dbContext.Products
            .Where(p => p.ProductStatus == "Active")
            .OrderByDescending(p => p.CreatedDate);
    }
}