using NUnit.Framework;
using Moq;
using Backend.Controllers;
using Backend.Interfaces;

namespace Backend.Tests;

[TestFixture]
public class UserTests
{
    private Mock<IUserService> _mockUserService;
    private UserController _userController;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>(MockBehavior.Default);
        _userController = new UserController(_mockUserService.Object);
    }
}