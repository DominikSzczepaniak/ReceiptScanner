using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{username}/{password}")]
    public async Task<IActionResult> GetUserData(string username, string password)
    {
        ///<summary>
        ///Api to get user data for username and password - mainly id
        ///</summary>
        ///<param name="username"></param>
        ///<param name="password"></param>
        User userData;
        try
        {
            userData = await _userService.GetUserData(username, password);
        }
        catch (ArgumentException ex)
        {
            throw new HttpRequestException(HttpRequestError.Unknown, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }

        if (userData == null)
        {
            return NotFound();
        }

        return Ok(userData); 
    }

    [HttpPost("{username}/{password}")]
    public async Task<IActionResult> RegisterUser(string username, string password)
    {
        ///<summary>
        ///Api to register user with username and password
        ///</summary>
        ///<param name="username"></param>
        ///<param name="password"></param>
        try
        {
            _userService.RegisterUser(username, password);
            return Ok();
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
    public async Task<IActionResult> DeleteUser(string username, string password)
    {
        ///<summary>
        /// Api to delete user by his username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        try
        {
            _userService.DeleteUser(username, password);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            throw new HttpRequestException(HttpRequestError.ConnectionError, ex.Message);
        }
    }
}