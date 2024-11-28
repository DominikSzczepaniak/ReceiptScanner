using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : Controller
{
    [HttpGet("{username}/{password}")]
    public async Task<IActionResult> GetUserData(string username, string password)
    {
        try
        {
            return Ok(await userService.GetUserData(username, password));
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

    [HttpPost("{username}/{password}")]
    public async Task<IActionResult> RegisterUser(string username, string password)
    {
        try
        {
            await userService.RegisterUser(username, password);
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

    [HttpDelete("{username}/{password}")]
    public async Task<IActionResult> DeleteUser(string username, string password)
    {
        try
        {
            await userService.DeleteUser(username, password);
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
}