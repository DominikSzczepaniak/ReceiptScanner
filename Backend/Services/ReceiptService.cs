﻿using Backend.Data;
using Backend.Models;

namespace Backend.Services;

public class ReceiptService
{
    private readonly IDatabaseHandler _databaseConnection;

    public ReceiptService(IDatabaseHandler databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public async Task<Receipt> GetReceiptData(int id)
    {
        try
        {
            Receipt receipt = await _databaseConnection.GetReceiptData(id);
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

    public async Task<List<Receipt>> GetReceiptsForUser(int ownerID)
    {
        try
        {
            List<Receipt> receipts = await _databaseConnection.GetReceiptsForUser(ownerID);
            return receipts;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task DeleteReceipt(int id)
    {
        try
        {
            await _databaseConnection.DeleteReceipt(id);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task AddReceipt(DateTime dateTime, string shopName, int ownerID)
    {
        try
        {
            _databaseConnection.AddReceipt(dateTime, shopName, ownerID);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
        
    }
}