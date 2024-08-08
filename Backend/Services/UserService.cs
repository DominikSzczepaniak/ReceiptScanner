using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class UserService(IDatabaseHandler databaseConnection)
    {
        public async Task<User> GetUserData(string username, string password)
        {
            try
            {
                var user = await databaseConnection.GetUserData(username, password);
                return user;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async void RegisterUser(string username, string password)
        {
            try
            {
                await databaseConnection.RegisterUser(username, password);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async void DeleteUser(string username, string password)
        {
            try
            {
                await databaseConnection.DeleteUser(username, password);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
