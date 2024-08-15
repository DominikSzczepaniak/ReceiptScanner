using Backend.Models;
using Npgsql;

namespace Backend.Data
{
    public class PostgresConnection : IDatabaseHandler, IDisposable
    {
        public static string ConnectionString;
        private NpgsqlConnection _connection;
        public void Dispose()
        {
            Disconnect();
        }
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

        public async void Disconnect()
        {
            try
            {
                if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
                {
                    await _connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task<User> GetUserData(string username, string password)
        {
            await using var cmd = new NpgsqlCommand("SELECT * FROM Users WHERE Username=@username AND Password=@password", _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await using var reader = await cmd.ExecuteReaderAsync();

            User user;
            if (await reader.ReadAsync())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                var dbUsername = reader.GetString(reader.GetOrdinal("Username"));
                var dbPassword = reader.GetString(reader.GetOrdinal("Password"));
                user = new User(id, dbUsername, dbPassword);
            }
            else
            {
                throw new ArgumentException("Credentials incorrect");
            }

            return user;
        }

        public async Task<int> GetMaximumUserId()
        {
            await using var cmd = new NpgsqlCommand("SELECT MAX(ID) FROM Users", _connection);
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
            var userid = await GetMaximumUserId() + 1;
            await using var checkExistance =
                new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password", _connection);
            checkExistance.Parameters.AddWithValue("username", username);
            checkExistance.Parameters.AddWithValue("password", password);

            var result = await checkExistance.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);
            if (count > 0)
            {
                throw new ArgumentException("User already exists");
            }
            

            await using var cmd = new NpgsqlCommand("INSERT INTO Users (ID, Username, Password) VALUES (@id, @username, @password)", _connection);
            cmd.Parameters.AddWithValue("id", userid);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteUser(string username, string password)
        {
            await using var cmd =
                new NpgsqlCommand("DELETE FROM Users WHERE Username=@username AND Password=@password", _connection);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Receipt> GetReceiptData(int id)
        {
            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipt WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            Receipt receipt;
            if (await reader.ReadAsync())
            {
                var dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                var shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                var ownerId = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                receipt = new Receipt(id, dateTime, shopName, ownerId);
            }
            else
            {
                throw new ArgumentException("No receipt with such id");
            }

            return receipt;
        }

        public async Task<List<Receipt>> GetReceiptsForUser(int userId)
        {
            await using var cmd = new NpgsqlCommand("SELECT * FROM Receipt WHERE OwnerID=@userId", _connection);
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

        public async Task DeleteReceipt(int id)
        {
            await using var cmd = new NpgsqlCommand("DELETE FROM Receipt WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            await using var cmd = new NpgsqlCommand("INSERT INTO Receipt VALUES (@date, @name, @id)", _connection);
            cmd.Parameters.AddWithValue("date", dateTime);
            cmd.Parameters.AddWithValue("name", shopName);
            cmd.Parameters.AddWithValue("id", ownerId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
        {
            await using var cmd =
                new NpgsqlCommand("INSERT INTO Product VALUES (@name, @price, @quantityWeight, @category, @ownerId)",
                    _connection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("quantityWeight", quantityWeight);
            cmd.Parameters.AddWithValue("category", category);
            cmd.Parameters.AddWithValue("ownerId", ownerId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            await using var cmd = new NpgsqlCommand("SELECT * FROM Product WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            Product product;
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
                product = new Product(id, name, quantityWeight, price, ownerId, category);
            }
            else
            {
                throw new ArgumentException("No product with such id");
            }

            return product;
        }

        public async Task<List<Product>> GetReceiptProducts(int receiptId)
        {
            var productIds = await GetProductsIdForReceipt(receiptId);
            var result = new List<Product>();
            foreach (var productId in productIds)
            {
                result.Add(await GetProduct(productId));
            }

            return result;
        }

        public async Task DeleteProduct(int id)
        {
            await using var cmd = new NpgsqlCommand("DELETE FROM Product WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteReceiptProducts(int receiptId)
        {
            var products = await GetReceiptProducts(receiptId);
            foreach (var product in products)
            {
                await DeleteProduct(product.Id);
            }
        }

        private async Task<List<int>> GetProductsIdForReceipt(int receiptId)
        {
            await using var cmd = new NpgsqlCommand("SELECT * FROM ReceiptToProducts WHERE ReceiptID=@receiptId", _connection);
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
            List<Receipt> result = new List<Receipt>();
            await using var cmd =
                new NpgsqlCommand("SELECT * FROM Receipt WHERE Date >= @startDate AND Date <= @endDate AND OwnerId = @ownerId", _connection);
            cmd.Parameters.AddWithValue("startDate", startDate.ToString("d"));
            cmd.Parameters.AddWithValue("endDate", endDate.ToString("d"));
            cmd.Parameters.AddWithValue("ownerId", ownerId);
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

        private async Task TryToConnect()
        {
            if (!await ConnectAsync())
            {
                throw new InvalidOperationException("Could not establish a connection to the database");
            }
        }
    }
}
