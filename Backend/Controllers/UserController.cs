using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;



namespace Backend.Controllers
{
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
                userData = await _userService.GetUser(username, password);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
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
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }


}
