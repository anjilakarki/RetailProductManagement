

using RetailProductManagement.Core.Entities;

namespace RetailProductManagement.Core.Contracts.Repositories;

public interface IProductRepository: IBaseRepository<Product>
{
   //Task<List<Product>> GetActiveProducts();
   IQueryable<Product> GetActiveProducts();
}