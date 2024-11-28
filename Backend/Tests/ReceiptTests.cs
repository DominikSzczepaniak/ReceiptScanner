using NUnit.Framework;
using Moq;
using Backend.Controllers;
using Backend.Services;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using NUnit.Framework.Legacy;

namespace Backend.Tests;

[TestFixture]
public class ReceiptTests
{
    private Mock<IReceiptService> _mockReceiptService;
    private Mock<IProductService> _mockProductService;
    private Mock<IReceiptProductConnectionService> _mockReceiptProductConnectionService;
    private ReceiptController _receiptController;

    [SetUp]
    public void Setup()
    {
        _mockReceiptService = new Mock<IReceiptService>(MockBehavior.Default);
        _mockProductService = new Mock<IProductService>(MockBehavior.Default);
        _mockReceiptProductConnectionService = new Mock<IReceiptProductConnectionService>(MockBehavior.Default);
        _receiptController = new ReceiptController(_mockReceiptService.Object, _mockProductService.Object, _mockReceiptProductConnectionService.Object);
    }

    [Test]
    public async Task GetReceiptData_WhenReceiptExists_ReturnsReceipt()
    {
        var receipt = new Receipt(1, DateTime.Now, "TestShop", 1);
        _mockReceiptService.Setup(x => x.GetReceiptData(1)).ReturnsAsync(receipt);
        
        var result = await _receiptController.GetReceiptData(1) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode);
        ClassicAssert.AreEqual(receipt, result.Value);
    }
}