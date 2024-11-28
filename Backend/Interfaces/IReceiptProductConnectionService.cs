namespace Backend.Interfaces;

public interface IReceiptProductConnectionService
{
    Task AddProductReceiptConnection(int productId, int receiptId);
    Task DeleteProductReceiptConnection(int receiptId, int productId);
    Task DeleteAllProductConnections(int productId);
    Task DeleteAllReceiptConnections(int id);
}