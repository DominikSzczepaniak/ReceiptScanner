using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class UserService(IDatabaseHandler databaseConnection)
    {
        public async Task<User> GetUserData(string username, string password)
        {
            var user = await databaseConnection.GetUserData(username, password);
            return user;
        }

        public async void RegisterUser(string username, string password)
        {
            await databaseConnection.RegisterUser(username, password);
        }

        public async void DeleteUser(string username, string password)
        {
            await databaseConnection.DeleteUser(username, password);
        }
    }
}
