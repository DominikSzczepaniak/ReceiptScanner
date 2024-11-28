using Backend.Models;

namespace Backend.Interfaces;

public interface IProductService
{
    Task<int> AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId);
    Task<Product> GetProduct(int productId);
    Task<List<Product>> GetReceiptProducts(int receiptId);
    Task DeleteProduct(int productId);
}