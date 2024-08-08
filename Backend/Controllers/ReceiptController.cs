using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceiptController : Controller
{
    private readonly ReceiptService _receiptService;
    private readonly ProductService _productService;
    public ReceiptController(ReceiptService receiptService, ProductService productService)
    {
        _receiptService = receiptService;
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceiptData(int id)
    {
        try
        {
            Receipt receipt = await _receiptService.GetReceiptData(id);
            return Ok(receipt);
        }
        catch (ArgumentException ex)
        {
            throw new HttpRequestException(HttpRequestError.Unknown, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpGet("receiptList/{ownerid}")]
    public async Task<IActionResult> GetReceiptsForUser(int ownerid)
    {
        try
        {
            List<Receipt> receipts = await _receiptService.GetReceiptsForUser(ownerid);
            return Ok(receipts);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReceipt(int id)
    {
        try
        {
            await _receiptService.DeleteReceipt(id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message); 
        }
    }

    [HttpPost("{ownerId}")]
    public async Task<IActionResult> AddReceipt([FromBody] JsonReceipt order, int ownerId)
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
        try
        {
            await _receiptService.AddReceipt(order.Date, order.ShopName, ownerId);
            foreach (var item in order.Items)
            {
                await _productService.AddProduct(item.Name, item.Price, item.QuantityWeight, item.Category, ownerId);
            }
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }
}