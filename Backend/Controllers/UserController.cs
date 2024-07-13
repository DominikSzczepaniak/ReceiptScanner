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
    }


}
