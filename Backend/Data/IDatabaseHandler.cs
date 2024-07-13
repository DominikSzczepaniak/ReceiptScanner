using Backend.Models;

namespace Backend.Data
{
    public interface IDatabaseHandler
    {
        /// <summary>
        /// Database interface for operations needed for API
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<User> GetUser(string username, string password);

        public Task<bool> RegisterUser(string username, string password);

    }
}
