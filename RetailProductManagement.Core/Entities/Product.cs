namespace RetailProductManagement.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ProductStatus { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; } 
}