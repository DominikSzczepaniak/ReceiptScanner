using Backend.Models;

namespace Backend.Data
{
    public interface IDatabaseHandler
    {
        public Task<User> GetUser(string username, string password);

        public Task<bool> RegisterUser(string username, string password);

    }
}
