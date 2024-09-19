using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class ReceiptService(IDatabaseHandler databaseConnection, ProductService productService)
{
    public async Task<Receipt> GetReceiptData(int id)
    {
        var receipt = await databaseConnection.GetReceiptData(id);
        return receipt;
    }

    public async Task<List<Receipt>> GetReceiptsForUser(int ownerId)
    {
        return await databaseConnection.GetReceiptsForUser(ownerId);
    }

    public async Task<List<Receipt>> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
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

    public async Task DeleteReceipt(int id) //TODO delete also from ReceiptToProducts database
    {
        var products = await productService.GetReceiptProducts(id);
        foreach (var product in products)
        {
            await databaseConnection.DeleteProduct(product.Id);
        }
        await databaseConnection.DeleteReceipt(id);
    }

    public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
    {
        await databaseConnection.AddReceipt(dateTime, shopName, ownerId);
    }
}