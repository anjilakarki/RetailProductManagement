using Microsoft.AspNetCore.Mvc;
using RetailProductManagement.Core.Contracts.Services;
using RetailProductManagement.Core.Entities;

namespace RetailProductManagement.API.Controllers;

// Attribute Routing
[Route("api/[controller]")]
[ApiController]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductController(IProductService productService)
    {
       _productService = productService;
    }
    
    // Get active products
    [HttpGet]
    public async Task<IActionResult> GetActiveProducts([FromQuery] string name = null, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var products = await _productService.GetActiveProducts(name, minPrice, maxPrice, startDate, endDate);
        if (products == null)
        {
            return NoContent();
        }
        return Ok(products);
    }
    
    // Create Product
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            var createdProduct = await _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetActiveProducts), new { id = createdProduct.Id }, createdProduct);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Update Product
        [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product productUpdate)
    {
        try
        {
            await _productService.UpdateProduct(id, productUpdate);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Delete Product
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await _productService.DeleteProduct(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
}