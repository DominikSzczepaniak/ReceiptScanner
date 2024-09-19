using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class ProductService(IDatabaseHandler databaseConnection)
{
    public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
    {
        await databaseConnection.AddProduct(name, price, quantityWeight, category, ownerId);
    }

    public async Task<Product> GetProduct(int productId)
    {
        return await databaseConnection.GetProduct(productId);
    }

    public async Task<List<Product>> GetReceiptProducts(int receiptId)
    {
        var productIds = await databaseConnection.GetProductsIdForReceipt(receiptId);
        var result = new List<Product>();
        foreach (var productId in productIds)
        {
            result.Add(await GetProduct(productId));
        }

        return result;
    }

    public async Task DeleteProduct(int productId) //TODO delete also from ReceiptToProducts database
    {
        await databaseConnection.DeleteProduct(productId);
    }
}