using Backend.Services;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService) : Controller
{
    [HttpGet("{username}/{password}")]
    public async Task<IActionResult> GetUserData(string username, string password)
    {
        ///<summary>
        ///Api to get user data for username and password - mainly id
        ///</summary>
        ///<param name="username"></param>
        ///<param name="password"></param>
        try
        {
            return Ok(await userService.GetUserData(username, password));
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

    [HttpPost("{username}/{password}")]
    public Task<IActionResult> RegisterUser(string username, string password)
    {
        ///<summary>
        ///Api to register user with username and password
        ///</summary>
        ///<param name="username"></param>
        ///<param name="password"></param>
        try
        {
            userService.RegisterUser(username, password);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
        catch (ArgumentException ex)
        {
            throw new HttpRequestException(HttpRequestError.Unknown, ex.Message);
        }
    }

    [HttpDelete("{username}/{password}")]
    public Task<IActionResult> DeleteUser(string username, string password)
    {
        try
        {
            userService.DeleteUser(username, password);
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }
}