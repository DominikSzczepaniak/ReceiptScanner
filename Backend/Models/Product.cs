namespace Backend.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Price { get; set; }
        public int OwnerID { get; set; }
        //public string Category { get; set; }

        public Product(int id, string name, double amount, int price, int ownerid)
        {
            this.Id = id;
            this.Name = name;
            this.Amount = amount;
            this.Price = price;
            this.OwnerID = ownerid;
        }
        
    }
}
