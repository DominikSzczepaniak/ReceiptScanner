using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class ReceiptService(IDatabaseHandler databaseConnection)
{
    public async Task<Receipt> GetReceiptData(int id)
    {
        try
        {
            var receipt = await databaseConnection.GetReceiptData(id);
            return receipt;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public async Task<List<Receipt>> GetReceiptsForUser(int ownerId)
    {
        try
        {
            var receipts = await databaseConnection.GetReceiptsForUser(ownerId);
            return receipts;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task DeleteReceipt(int id) //TODO delete also from ReceiptToProducts database
    {
        await databaseConnection.DeleteReceiptProducts(id);
        await databaseConnection.DeleteReceipt(id);
    }

    public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
    {
        try
        {
            await databaseConnection.AddReceipt(dateTime, shopName, ownerId);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}