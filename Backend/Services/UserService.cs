using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class UserService(IDatabaseHandler databaseConnection) : IUserService
    {
        public async Task<User> GetUserData(string username, string password)
        {
            return await databaseConnection.GetUserData(username, password);
        }

        public async Task RegisterUser(string username, string password)
        {
            await databaseConnection.RegisterUser(username, password);
        }

        public async Task DeleteUser(string username, string password)
        {
            await databaseConnection.DeleteUser(username, password);
        }
    }
}
