using RetailProductManagement.Core.Entities;

namespace RetailProductManagement.Core.Contracts.Services;

public interface IProductService
{
    Task<List<Product>> GetActiveProducts(string name , decimal? minPrice , decimal? maxPrice ,
        DateTime? startDate , DateTime? endDate);

    Task<Product> CreateProduct(Product product);
    Task UpdateProduct(int id, Product productUpdate);
    Task DeleteProduct(int id);
}