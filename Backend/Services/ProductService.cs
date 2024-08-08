using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class ProductService
{
    private readonly IDatabaseHandler _databaseConnection;

    public ProductService(IDatabaseHandler databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }
    public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
    {
        await _databaseConnection.AddProduct(name, price, quantityWeight, category, ownerId);
    }

    public async Task<Product> GetProduct(int productId)
    {
        return await _databaseConnection.GetProduct(productId);
    }

    public async Task<List<Product>> GetReceiptProducts(int receiptId)
    {
        return await _databaseConnection.GetReceiptProducts(receiptId);
    }

    public async Task DeleteProduct(int productId) //TODO delete also from ReceiptToProducts database
    {
        await _databaseConnection.DeleteProduct(productId);
    }
}