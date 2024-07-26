using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class UserService
    {
        /// <summary>
        /// Class giving service for accessing, editing and deleting user data from database
        /// </summary>
        private readonly IDatabaseHandler _databaseConnection;

        public UserService(IDatabaseHandler databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<User> GetUserData(string username, string password)
        {
            try
            {
                User user = await _databaseConnection.GetUserData(username, password);
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
                await _databaseConnection.RegisterUser(username, password);
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
                await _databaseConnection.DeleteUser(username, password);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
