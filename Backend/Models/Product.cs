namespace Backend.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal QuantityWeight { get; set; }
        public decimal Price { get; set; }
        public int OwnerID { get; set; }
        public string? Category { get; set; }

        public Product(int id, string name, decimal quantityWeight, decimal price, int ownerid, string? category)
        {
            this.Id = id;
            this.Name = name;
            this.QuantityWeight = quantityWeight;
            this.Price = price;
            this.OwnerID = ownerid;
            this.Category = category;
        }
    }
}
