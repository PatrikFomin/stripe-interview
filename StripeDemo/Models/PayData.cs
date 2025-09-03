namespace StripeDemo.Models;

public class PayData
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Locale { get; set; } = "en";

    public int Prod1 { get; set; }
    public int Prod2 { get; set; }
    public int Prod3 { get; set; }
}