using Backend.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class PostgresConnection : IDatabaseHandler
    {
        private string ConnectionString = "Host=127.0.0.1;Port=5432;Username=ReceiptProject;Password=Receipts;Database=ReceiptProject;";
        private NpgsqlConnection _connection;

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _connection = new NpgsqlConnection(ConnectionString);
                await _connection.OpenAsync();
                return _connection.State == System.Data.ConnectionState.Open;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task<User> GetUserData(string username, string password)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT * FROM Users WHERE Username=@username AND Password=@password", _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await using var reader = await cmd.ExecuteReaderAsync();

            User user = null;
            if (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID"));
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
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT MAX(ID) FROM Users", _connection);
            await using var reader = await cmd.ExecuteReaderAsync();

            int id = 0; //if there's no users we want the first user to have id = 1
            if (await reader.ReadAsync())
            {
                id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            }

            Disconnect();
            return id;
        }

        public async Task RegisterUser(string username, string password)
        {
            int userid = await GetMaximumUserID() + 1;
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var checkExistance =
                new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password");
            checkExistance.Parameters.AddWithValue("username", username);
            checkExistance.Parameters.AddWithValue("password", password);

            var result = await checkExistance.ExecuteScalarAsync();
            int count = Convert.ToInt32(result);
            if (count > 0)
            {
                Disconnect();
                throw new ArgumentException("User already exists");
            }
            

            await using var cmd = new NpgsqlCommand("INSERT INTO Users (ID, Username, Password) VALUES (@id, @username, @password)", _connection);
            cmd.Parameters.AddWithValue("id", userid);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
        }

        public async Task DeleteUser(string username, string password)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");
            
            await using var cmd =
                new NpgsqlCommand("DELETE FROM Users WHERE Username=@username AND Password=@password",
                    _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
            
        }

        public async Task<Receipt> GetReceiptData(int id)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipts WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            Receipt receipt = null;
            if (await reader.ReadAsync())
            {
                DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                string shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                int ownerID = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                receipt = new Receipt(id, dateTime, shopName, ownerID);
            }
            else
            {
                throw new ArgumentException("No receipt with such id");
            }

            Disconnect();
            return receipt;
        }

        public async Task<List<Receipt>> GetReceiptsForUser(int userId)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipts WHERE OwnerID=@userId", _connection);
            cmd.Parameters.AddWithValue("userId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            List<Receipt> answer = new List<Receipt>();
            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID"));
                DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                string shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                int ownerID = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                answer.Add(new Receipt(id, dateTime, shopName, ownerID));
            }

            Disconnect();
            return answer;
        }

        public async Task DeleteReceipt(int id)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("DELETE FROM Receipts WHERE ID=@id");
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
        }

        public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            throw new NotImplementedException();
        }
    }
}
