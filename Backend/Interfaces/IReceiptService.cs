using Backend.Models;

namespace Backend.Interfaces;

public interface IReceiptService
{
    Task<Receipt> GetReceiptData(int id);
    Task<List<Receipt>> GetReceiptsForUser(int ownerId);
    Task<decimal> GetTotalSpendingBetweenDates(DateTime startDate, DateTime endDate, int ownerId);
    Task<(List<Receipt>, decimal)> GetMainPageData(DateTime startDate, DateTime endDate, int ownerId);
    Task DeleteReceipt(int id);
    Task<int> AddReceipt(DateTime dateTime, string shopName, int ownerId);
}