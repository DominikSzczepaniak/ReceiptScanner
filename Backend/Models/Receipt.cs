namespace Backend.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ShopName { get; set; }
        public int OwnerID { get; set; }

        public Receipt(int id, DateTime date, string shopName, int ownerID)
        {
            Id = id;
            Date = date;
            ShopName = shopName;
            OwnerID = ownerID;
        }
    }
}
