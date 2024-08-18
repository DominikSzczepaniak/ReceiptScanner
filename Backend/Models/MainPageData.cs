namespace Backend.Models;

public class MainPageData()
{ 
    public List<Receipt> Receipts { get; set; }
    public decimal Total { get; set; }

    public MainPageData(List<Receipt> receipts, decimal total) : this()
    {
        Receipts = receipts;
        Total = total;
    }
}