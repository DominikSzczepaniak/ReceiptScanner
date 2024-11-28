using Backend.Models;

namespace Backend.Interfaces;

public interface IUserService
{
    Task<User> GetUserData(string username, string password);
    Task RegisterUser(string username, string password);
    Task DeleteUser(string username, string password);
}