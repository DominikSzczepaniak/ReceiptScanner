namespace Backend.Models
{
    public class ReceiptProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ReceiptId { get; set; }

        public ReceiptProduct(int id, int productId, int receiptId)
        {
            Id = id;
            ProductId = productId;
            ReceiptId = receiptId;
        }
    }
}
