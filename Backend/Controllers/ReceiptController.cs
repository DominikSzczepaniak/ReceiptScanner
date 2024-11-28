using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceiptController(IReceiptService receiptService, IProductService productService, IReceiptProductConnectionService receiptProductConnectionService)
    : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceiptData(int id)
    {
        try
        {
            var receipt = await receiptService.GetReceiptData(id);
            return Ok(receipt);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpGet("receipts/{ownerId}")] //TODO: Limit number of receipts retrieved from database and only load more if user changes table in frontend
    public async Task<IActionResult> GetReceiptsForUser(int ownerId)
    {
        try
        {
            var receipts = await receiptService.GetReceiptsForUser(ownerId);
            return Ok(receipts);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpGet("totalSpending/{startDate}/{endDate}/{ownerId}")]
    public async Task<IActionResult> GetTotalSpendingBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
    {
        try
        {
            var result = await receiptService.GetTotalSpendingBetweenDates(startDate, endDate, ownerId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpGet("receipts/{startDate}/{endDate}/{ownerId}")]
    public async Task<IActionResult> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
    {
        try
        {
            var result = await GetReceiptsBetweenDates(startDate, endDate, ownerId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpGet("mainPageData/{startDate}/{endDate}/{ownerId}")]
    public async Task<IActionResult> GetMainPageData(DateTime startDate, DateTime endDate, int ownerId)
    {
        try
        {
            var result = await receiptService.GetMainPageData(startDate, endDate, ownerId);
            var receipts = result.Item1;
            var total = result.Item2;
            var response = new MainPageData(receipts, total);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpDelete("{receiptId}")]
    public async Task<IActionResult> DeleteReceipt(int receiptId)
    {
        try
        {
            await receiptService.DeleteReceipt(receiptId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message); 
        }
    }

    [HttpPost("{ownerId}")]
    public async Task<IActionResult> AddReceipt([FromBody] JsonReceipt order, int ownerId) //TODO delegate logic to ReceiptService 
    {
        //json body:
        //{
        // shopName: string,
        // date: DateTime
        // items: {
        //   name: string,
        //   quantity_weight: decimal,
        //   price: decimal,
        //   category: string
        // }
        //}
        if (order.ShopName == null || order.Date == null || order.Items == null || order.Items.Count == 0)
        {
            throw new HttpRequestException(HttpRequestError.Unknown, "Body is not correct");
        }
        try
        {
            var receiptId = await receiptService.AddReceipt(order.Date, order.ShopName, ownerId);
            foreach (var item in order.Items)
            {
                var productId = await productService.AddProduct(item.Name, item.Price, item.QuantityWeight, item.Category, ownerId);
                await receiptProductConnectionService.AddProductReceiptConnection(productId, receiptId);
            }
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }
}