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

        public async Task<User> GetUser(string username, string password)
        {
            try
            {
                User user = await _databaseConnection.GetUser(username, password);
                return user;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            try
            {
                bool result = await _databaseConnection.RegisterUser(username, password);
                return result;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
