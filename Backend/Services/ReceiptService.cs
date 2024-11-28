using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class ReceiptService(IDatabaseHandler databaseConnection, IProductService productService, IReceiptProductConnectionService receiptProductConnectionService) : IReceiptService
{
    public async Task<Receipt> GetReceiptData(int id)
    {
        return await databaseConnection.GetReceiptData(id);
    }

    public async Task<List<Receipt>> GetReceiptsForUser(int ownerId)
    {
        return await databaseConnection.GetReceiptsForUser(ownerId);
    }

    private async Task<List<Receipt>> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
    {
        return await databaseConnection.GetReceiptsBetweenDates(startDate, endDate, ownerId);
    }

    public async Task<decimal> GetTotalSpendingBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
    {
        var receipts = await GetReceiptsBetweenDates(startDate, endDate, ownerId);
        decimal result = 0;
        foreach (var receipt in receipts)
        {
            var products = await productService.GetReceiptProducts(receipt.Id);
            decimal total = 0;
            foreach (var product in products)
            {
                total += product.Price;
            }

            result += total;
        }

        return result;
    }

    public async Task<(List<Receipt>, decimal)> GetMainPageData(DateTime startDate, DateTime endDate, int ownerId)
    {
        var receipts = await GetReceiptsBetweenDates(startDate, endDate, ownerId);
        var totalSpending = await GetTotalSpendingBetweenDates(startDate, endDate, ownerId);
        return (receipts, totalSpending);
    }

    public async Task DeleteReceipt(int id)
    {
        await receiptProductConnectionService.DeleteAllReceiptConnections(id);
        var products = await productService.GetReceiptProducts(id);
        foreach (var product in products)
        {
            await databaseConnection.DeleteProduct(product.Id);
        }
        await databaseConnection.DeleteReceipt(id);
    }

    public async Task<int> AddReceipt(DateTime dateTime, string shopName, int ownerId)
    {
        return await databaseConnection.AddReceipt(dateTime, shopName, ownerId);
    }
}