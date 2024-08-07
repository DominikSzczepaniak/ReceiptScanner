namespace Backend.Models;

public class JsonReceipt
{
    public string ShopName { get; set; }
    public DateTime Date { get; set; }
    public List<JsonProduct> Items { get; set; }
}