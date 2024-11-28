using NUnit.Framework;
using Moq;
using Backend.Controllers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using NUnit.Framework.Legacy;

namespace Backend.Tests;

[TestFixture]
public class ProductTests
{
    private Mock<IProductService> _mockProductService;
    private Mock<IReceiptService> _mockReceiptService;
    private ProductController _productController;

    [SetUp]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>(MockBehavior.Default);
        _productController = new ProductController(_mockProductService.Object);
        _mockReceiptService = new Mock<IReceiptService>(MockBehavior.Default);
    }
    
    [Test]
    public async Task GetProduct_WhenProductExists_ReturnsProduct()
    {
        var product = new Product(1, "TestProduct", 10, 1, 1, "TestCategory");
        _mockProductService.Setup(x => x.GetProduct(1)).ReturnsAsync(product);
        
        var result = await _productController.GetProduct(1) as OkObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode);
        ClassicAssert.AreEqual(product, result.Value);
    }
    
    [Test]
    public async Task GetProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        _mockProductService.Setup(x => x.GetProduct(1)).ThrowsAsync(new ArgumentException("Product not found"));
        
        var result = await _productController.GetProduct(1) as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result.StatusCode);
        ClassicAssert.AreEqual("Product not found", result.Value);
    }
    
    [Test]
    public async Task DeleteProduct_WhenProductExists_ReturnsOk()
    {
        _mockProductService.Setup(x => x.DeleteProduct(1)).Returns(Task.CompletedTask);
        
        var result = await _productController.DeleteProduct(1) as OkResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(200, result.StatusCode);
    }
    
    [Test]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        _mockProductService.Setup(x => x.DeleteProduct(1)).ThrowsAsync(new ArgumentException("Product not found"));
        
        var result = await _productController.DeleteProduct(1) as NotFoundObjectResult;
        
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(404, result.StatusCode);
        ClassicAssert.AreEqual("Product not found", result.Value);
    }
    
}