using Backend.Models;
using Npgsql;

namespace Backend.Data
{
    public class PostgresConnection(PostgresConnectionPool connectionPool) : IDatabaseHandler, IDisposable
    {
        public void Dispose()
        {
            connectionPool.Dispose();
        }
        
        private async Task<NpgsqlConnection> GetConnectionAsync()
        {
            return await connectionPool.GetConnectionAsync();
        }

        public async Task<User> GetUserData(string username, string password)
        {
            if (!await CheckUserExistanceForCredentials(username, password))
            {
                throw new ArgumentException("No user for these credentials");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM Users WHERE Username=@username AND Password=@password", connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                var dbUsername = reader.GetString(reader.GetOrdinal("Username"));
                var dbPassword = reader.GetString(reader.GetOrdinal("Password"));
                return new User(id, dbUsername, dbPassword);
            }

            throw new ArgumentException("No user with these credentials");
        }

        public async Task<bool> CheckUserExistanceForCredentials(string username, string password)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM User WHERE Username=@username AND Password=@password", connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            
            var result = await cmd.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);
            return count > 0;
        }

        public async Task<int> GetMaximumUserId() //TODO create sequence and get id from it
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT MAX(ID) FROM Users", connection);
            await using var reader = await cmd.ExecuteReaderAsync();

            var id = 0; //if there's no users we want the first user to have id = 1
            if (await reader.ReadAsync())
            {
                id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            }

            return id;
        }

        public async Task RegisterUser(string username, string password)
        {
            if (await CheckUserExistanceForCredentials(username, password))
            {
                throw new ArgumentException("User already exists");
            }
            await using var connection = await GetConnectionAsync();
            var userid = await GetMaximumUserId() + 1;
            
            await using var cmd = new NpgsqlCommand("INSERT INTO Users (ID, Username, Password) VALUES (@id, @username, @password)", connection);
            cmd.Parameters.AddWithValue("id", userid);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteUser(string username, string password)
        {
            if (!await CheckUserExistanceForCredentials(username, password))
            {
                throw new ArgumentException("No user with these credentials");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd =
                new NpgsqlCommand("DELETE FROM Users WHERE Username=@username AND Password=@password", connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CheckReceiptExistance(int receiptId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Receipt WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", receiptId);
            
            var result = await cmd.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);
            return count > 0;
        }

        public async Task<Receipt> GetReceiptData(int receiptId)
        {
            if (!await CheckReceiptExistance(receiptId))
            {
                throw new ArgumentException("No receipt with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipt WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", receiptId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                var shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                var ownerId = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                return new Receipt(receiptId, dateTime, shopName, ownerId);
            }
            throw new ArgumentException("No receipt with this id");
        }

        public async Task<List<Receipt>> GetReceiptsForUser(int userId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipt WHERE OwnerID=@userId", connection);
            cmd.Parameters.AddWithValue("userId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<Receipt>();
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                var dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                var shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                var ownerId = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                result.Add(new Receipt(id, dateTime, shopName, ownerId));
            }
            
            return result;
        }

        public async Task DeleteReceipt(int receiptId)
        {
            if (!await CheckReceiptExistance(receiptId))
            {
                throw new ArgumentException("No receipt with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM Receipt WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", receiptId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("INSERT INTO Receipt VALUES (@date, @name, @id)", connection);
            cmd.Parameters.AddWithValue("date", dateTime);
            cmd.Parameters.AddWithValue("name", shopName);
            cmd.Parameters.AddWithValue("id", ownerId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd =
                new NpgsqlCommand("INSERT INTO Product VALUES (@name, @price, @quantityWeight, @category, @ownerId)",
                    connection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("quantityWeight", quantityWeight);
            cmd.Parameters.AddWithValue("category", category);
            cmd.Parameters.AddWithValue("ownerId", ownerId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CheckProductExistance(int productId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Product WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", productId);
            
            var result = await cmd.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);
            return count > 0;
        }

        public async Task<Product> GetProduct(int productId)
        {
            if(!await CheckProductExistance(productId))
            {
                throw new ArgumentException("No product with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM Product WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", productId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var name = reader.GetString(reader.GetOrdinal("Name"));
                var quantityWeight = reader.GetDecimal(reader.GetOrdinal("QuantityWeight"));
                var price = reader.GetDecimal(reader.GetOrdinal("Price"));
                var ownerId = reader.GetInt32(reader.GetOrdinal("OwnerID"));

                string? category = null;
                if (!reader.IsDBNull(reader.GetOrdinal("Category")))
                {
                    category = reader.GetString(reader.GetOrdinal("Category"));
                }
                return new Product(productId, name, quantityWeight, price, ownerId, category);
            }
            throw new ArgumentException("No product with this id");
        }

        public async Task DeleteProduct(int productId)
        {
            if (!await CheckProductExistance(productId))
            {
                throw new ArgumentException("No product with such id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM Product WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", productId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<int>> GetProductsIdForReceipt(int receiptId)
        {
            if (!await CheckReceiptExistance(receiptId))
            {
                throw new ArgumentException("No receipt with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT * FROM ReceiptToProducts WHERE ReceiptID=@receiptId", connection);
            cmd.Parameters.AddWithValue("receiptId", receiptId);

            await using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<int>();
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ProductID"));
                result.Add(id);
            }

            return result;
        }

        public async Task<List<Receipt>> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId)
        {
            await using var connection = await GetConnectionAsync();
            List<Receipt> result = new List<Receipt>();
            await using var cmd =
                new NpgsqlCommand($"SELECT * FROM Receipt WHERE Date >= '{startDate:yyyy-MM-dd}' AND Date <= '{endDate:yyyy-MM-dd}' AND OwnerId = {ownerId}", connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                var date = reader.GetDateTime(reader.GetOrdinal("Date"));
                var shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                result.Add(new Receipt(id, date, shopName, ownerId));
            }

            return result;
        }
    
        public async Task DeleteProductReceiptConnection(int productId, int receiptId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM ReceiptToProducts WHERE ReceiptId=@receiptId AND ProductId=@productId", connection);
            cmd.Parameters.AddWithValue("receiptId", receiptId);
            cmd.Parameters.AddWithValue("productId", productId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
