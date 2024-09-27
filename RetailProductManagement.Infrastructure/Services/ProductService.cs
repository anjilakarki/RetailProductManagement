using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Core.Contracts.Services;
using RetailProductManagement.Core.Entities;
using RetailProductManagement.Infrastructure.Repositories;

namespace RetailProductManagement.Infrastructure.Services;

public class ProductService: IProductService
{

    private readonly IProductRepository productRepository;
    private readonly IApprovalQueueService _approvalQueueService;

    public ProductService(IProductRepository _productRepository, IApprovalQueueService approvalQueueService)
    {
        productRepository = _productRepository;
        _approvalQueueService = approvalQueueService;
    }

    public async Task<List<Product>> GetActiveProducts(string name = null, decimal? minPrice = null, decimal? maxPrice = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        
        var products =   productRepository.GetActiveProducts();

        if (products == null || !products.Any()) 
        {
            return new List<Product>(); 
        }
        
        //var query = products.AsQueryable(); 

        if (!string.IsNullOrEmpty(name))
        {
            products = products.Where(p => p.Name.Contains(name));
        }
        if (minPrice.HasValue && maxPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }
        if (startDate.HasValue && endDate.HasValue)
        {
            products = products.Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate);
        }

        // Order by latest first
        products = products.OrderByDescending(p => p.CreatedDate);

        return await products.ToListAsync(); // Await the final conversion to a List
    }

    public async Task<Product> CreateProduct(Product product)
    {
        if (product.Price > 10000)
        {
            throw new InvalidOperationException("Product price cannot exceed 10000 dollars.");
        }
        await productRepository.AddAsync(product);

        // Push to approval queue if price is more than 5000
        if (product.Price > 5000)
        {
            await _approvalQueueService.PushToApprovalQueue(product, "Create", "price is more than $5000");
        }

       
        return product;
    }

    public async Task UpdateProduct(int id, Product productUpdate)
    {
        if (productUpdate.Price > 10000)
        {
            throw new InvalidOperationException("Product price cannot exceed 10000 dollars.");
        }
        
        var existingProduct = await productRepository.GetByIdAsync(id); 
        if (existingProduct == null) throw new KeyNotFoundException("Product not found");

        // Check price increase by more than 50%
        if (productUpdate.Price > existingProduct.Price * 1.5m)
        {
            await _approvalQueueService.PushToApprovalQueue(existingProduct, "Update", "Price more than 50% of previous");
        }
        // Check price > 5000
        if (productUpdate.Price > 5000)
        {
            await _approvalQueueService.PushToApprovalQueue(existingProduct, "Update", "Price more than $5000");
        }

        await productRepository.UpdateAsync(productUpdate);
    }

    public async Task DeleteProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null) throw new KeyNotFoundException("Product not found");

        await _approvalQueueService.PushToApprovalQueue(product, "Delete", "Request to delete the product");
    }
}

