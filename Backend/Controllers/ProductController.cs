using Backend.Services;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ProductService productService) : Controller
{
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        try
        {
            var product = await productService.GetProduct(productId);
            return Ok(product);
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
    
    [HttpGet("receipt/{receiptId}")]
    public async Task<IActionResult> ReceiptProducts(int receiptId)
    {
        try
        {
            return Ok(await productService.GetReceiptProducts(receiptId));
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

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        try
        {
            await productService.DeleteProduct(productId);
            return Ok();
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
}