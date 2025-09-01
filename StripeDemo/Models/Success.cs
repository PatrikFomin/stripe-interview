namespace StripeDemo.Models;

public class Success
{
    public required string Status { get; set; }
    public decimal Amount { get; set; }
}