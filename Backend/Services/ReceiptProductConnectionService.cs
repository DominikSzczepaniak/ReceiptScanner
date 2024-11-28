using Backend.Data;
using Backend.Interfaces;

namespace Backend.Services;

public class ReceiptProductConnectionService(IDatabaseHandler databaseConnection) : IReceiptProductConnectionService
{
    public async Task AddProductReceiptConnection(int productId, int receiptId)
    {
        await databaseConnection.AddProductReceiptConnection(productId, receiptId);
    }
    
    public async Task DeleteProductReceiptConnection(int receiptId, int productId)
    {
        await databaseConnection.DeleteProductReceiptConnection(receiptId, productId);
    }
    
    public async Task DeleteAllProductConnections(int productId)
    {
        await databaseConnection.DeleteAllProductConnections(productId);
    }

    public async Task DeleteAllReceiptConnections(int id)
    {
        await databaseConnection.DeleteAllReceiptConnections(id);
    }
}