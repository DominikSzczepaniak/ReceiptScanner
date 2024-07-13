using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Backend.Data
{
    public class PostgresConnection : IDatabaseHandler
    {
        private string ConnectionString { get; set; }
        private NpgsqlConnection _connection;

        public PostgresConnection(string conString)
        {
            ConnectionString = conString;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _connection = new NpgsqlConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
            await _connection.OpenAsync();
            return _connection.State == System.Data.ConnectionState.Open;
        }

        public void Disconnect()
        {
            try
            {
                _connection.Close();
                return;
            } catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }

        public async Task<User> GetUser(string username, string password)
        {
            await ConnectAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM Users WHERE Username=@username AND Password=@password", _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await using var reader = await cmd.ExecuteReaderAsync();

            User user = null;
            if (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("Id"));
                string dbUsername = reader.GetString(reader.GetOrdinal("Username"));
                string dbPassword = reader.GetString(reader.GetOrdinal("Password"));
                user = new User(id, dbUsername, dbPassword);
            }
            else
            {
                throw new ArgumentException("Credentials incorrect");
            }

            Disconnect();
            return user;
        }

        public async Task<int> GetMaximumUserID()
        {
            await ConnectAsync();
            await using var cmd = new NpgsqlCommand("SELECT MAX(Id) FROM Users");

            await using var reader = await cmd.ExecuteReaderAsync();
            int id = 1; //if theres no users we want first user to have id = 1
            if (await reader.ReadAsync())
            {
                id = reader.GetInt32(reader.GetOrdinal("Id"));
            }
            Disconnect();
            return id;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            int userid = await GetMaximumUserID()+1;
            await ConnectAsync();
            await using var cmd = new NpgsqlCommand("INSERT INTO Users VALUES (@id, @username, @password)");
            cmd.Parameters.AddWithValue("id", userid);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
            return true;
        }
    }
}
