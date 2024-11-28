using Backend.Models;

namespace Backend.Data
{
    public interface IDatabaseHandler
    {
        Task<User> GetUserData(string username, string password);
        Task<int> RegisterUser(string username, string password);
        Task DeleteUser(string username, string password);
        Task<Receipt> GetReceiptData(int id);
        Task<List<Receipt>> GetReceiptsForUser(int userId);
        Task DeleteReceipt(int id);
        Task<int> AddReceipt(DateTime dateTime, string shopName, int ownerId);
        Task<int> AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId);
        Task<Product> GetProduct(int id);
        Task<List<int>> GetProductsIdForReceipt(int receiptId);
        Task<List<Receipt>> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId);
        Task DeleteProduct(int id);
        Task AddProductReceiptConnection(int productId, int receiptId);
        Task DeleteAllProductConnections(int productId);
        Task DeleteAllReceiptConnections(int id);
        Task DeleteProductReceiptConnection(int receiptId, int productId);
    }
}
