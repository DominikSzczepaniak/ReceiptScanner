using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly ReceiptService _receiptService;
        public ReceiptController(ReceiptService receiptService)
        {
            _receiptService = receiptService;
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

        [HttpPost("{dateTime}/{shopName}/{ownerId}")]
        public async Task<IActionResult> AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            try
            {
                await _receiptService.AddReceipt(dateTime, shopName, ownerId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
            }
        }

        
    }


}
