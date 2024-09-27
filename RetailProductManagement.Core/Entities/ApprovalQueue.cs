namespace RetailProductManagement.Core.Entities;

public class ApprovalQueue
{
    public int Id { get; set; }
    public string RequestType { get; set; } = string.Empty; // Request type (Create, Update, Delete)
    public string RequestReason { get; set; } = string.Empty; // Reason for request
    public DateTime RequestDate { get; set; } = DateTime.Now; 
    //Navigation Property
    public int ProductId { get; set; }
    public Product Product { get; set; }
}