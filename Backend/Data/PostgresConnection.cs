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
            if (!await CheckUserExistenceForCredentials(username, password))
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

        private async Task<bool> CheckUserExistenceForCredentials(string username, string password)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password", connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            
            var result = await cmd.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);
            return count > 0;
        }

        public async Task<int> RegisterUser(string username, string password)
        {
            if (await CheckUserExistenceForCredentials(username, password))
            {
                throw new ArgumentException("User already exists");
            }
            await using var connection = await GetConnectionAsync();
            
            await using var cmd = new NpgsqlCommand("INSERT INTO Users (Username, Password) VALUES (@username, @password) RETURNING ID", connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task DeleteUser(string username, string password)
        {
            if (!await CheckUserExistenceForCredentials(username, password))
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

        private async Task<bool> CheckReceiptExistence(int receiptId)
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
            if (!await CheckReceiptExistence(receiptId))
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
            if (!await CheckReceiptExistence(receiptId))
            {
                throw new ArgumentException("No receipt with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM Receipt WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", receiptId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("INSERT INTO Receipt (Date, Shopname, OwnerID) VALUES (@date, @name, @id) RETURNING ID", connection);
            cmd.Parameters.AddWithValue("date", dateTime);
            cmd.Parameters.AddWithValue("name", shopName);
            cmd.Parameters.AddWithValue("id", ownerId);

            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task<int> AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
        {
            await using var connection = await GetConnectionAsync();
            await using var cmd =
                new NpgsqlCommand("INSERT INTO Product (Name, QuantityWeight, Price, OwnerID, Category) VALUES (@name, @quantityWeight, @price, @ownerId, @category) RETURNING ID",
                    connection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("quantityWeight", quantityWeight);
            cmd.Parameters.AddWithValue("ownerId", ownerId);
            cmd.Parameters.AddWithValue("category", category);

            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        private async Task<bool> CheckProductExistence(int productId)
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
            if(!await CheckProductExistence(productId))
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
            if (!await CheckProductExistence(productId))
            {
                throw new ArgumentException("No product with such id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("DELETE FROM Product WHERE ID=@id", connection);
            cmd.Parameters.AddWithValue("id", productId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddProductReceiptConnection(int productId, int receiptId)
        {
            if (!await CheckReceiptExistence(receiptId))
            {
                throw new ArgumentException("No receipt with this id");
            }
            if (!await CheckProductExistence(productId))
            {
                throw new ArgumentException("No product with this id");
            }
            await using var connection = await GetConnectionAsync();
            await using var cmd = new NpgsqlCommand("INSERT INTO ReceiptToProducts (ReceiptID, ProductID) VALUES (@receiptId, @productId)", connection);
            cmd.Parameters.AddWithValue("receiptId", receiptId);
            cmd.Parameters.AddWithValue("productId", productId);
            
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<int>> GetProductsIdForReceipt(int receiptId)
        {
            if (!await CheckReceiptExistence(receiptId))
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
