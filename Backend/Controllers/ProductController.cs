using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        try
        {
            Product product = await _productService.GetProduct(productId);
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
    
    [HttpGet("receiptProducts/{receiptId}")]
    public async Task<IActionResult> ReceiptProducts(int receiptId)
    {
        try
        {
            return Ok(await _productService.GetReceiptProducts(receiptId));
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        try
        {
            await _productService.DeleteProduct(productId);
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