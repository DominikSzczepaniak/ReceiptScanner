using Backend.Models;
using Npgsql;

namespace Backend.Data
{
    public class PostgresConnection : IDatabaseHandler
    {
        public static string ConnectionString;// = "Host=127.0.0.1;Port=5432;Username=ReceiptProject;Password=Receipts;Database=ReceiptProject;";
        private NpgsqlConnection _connection;
        public async Task<bool> ConnectAsync()
        {
            try
            {
                _connection = new NpgsqlConnection(ConnectionString);
                await _connection.OpenAsync();
                Console.WriteLine("connected");
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
                    Console.WriteLine("disconnected");
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
            Console.WriteLine(userid);
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");
            await using var checkExistance =
                new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@username AND Password=@password", _connection);
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
                new NpgsqlCommand("DELETE FROM Users WHERE Username=@username AND Password=@password", _connection);
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

            Receipt receipt;
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
            List<Receipt> result = new List<Receipt>();
            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID"));
                DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("Date"));
                string shopName = reader.GetString(reader.GetOrdinal("Shopname"));
                int ownerID = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                result.Add(new Receipt(id, dateTime, shopName, ownerID));
            }

            Disconnect();
            return result;
        }

        public async Task DeleteReceipt(int id)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("DELETE FROM Receipts WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
        }

        public async Task AddReceipt(DateTime dateTime, string shopName, int ownerId)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("INSERT INTO Receipts VALUES (@date, @name, @id)", _connection);
            cmd.Parameters.AddWithValue("date", dateTime);
            cmd.Parameters.AddWithValue("name", shopName);
            cmd.Parameters.AddWithValue("id", ownerId);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
        }

        public async Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd =
                new NpgsqlCommand("INSERT INTO Products VALUES (@name, @price, @quantityWeight, @category, @ownerId)",
                    _connection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("quantityWeight", quantityWeight);
            cmd.Parameters.AddWithValue("category", category);
            cmd.Parameters.AddWithValue("ownerId", ownerId);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
        }

        public async Task<Product> GetProduct(int id)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT * FROM Products WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            Product product;
            if (await reader.ReadAsync())
            {
                string name = reader.GetString(reader.GetOrdinal("Name"));
                decimal quantityWeight = reader.GetDecimal(reader.GetOrdinal("QuantityWeight"));
                decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                int ownerID = reader.GetInt32(reader.GetOrdinal("OwnerID"));
                string category = reader.GetString(reader.GetOrdinal("Category"));
                product = new Product(id, name, quantityWeight, price, ownerID, category);
            }
            else
            {
                throw new ArgumentException("No product with such id");
            }

            Disconnect();
            return product;
        }

        public async Task<List<Product>> GetReceiptProducts(int receiptId)
        {
            var productIds = await GetProductsIdForReceipt(receiptId);
            List<Product> result = new List<Product>();
            foreach (var productId in productIds)
            {
                result.Add(await GetProduct(productId));
            }

            return result;
        }

        public async Task DeleteProduct(int id)
        {
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("DELETE FROM Products WHERE ID=@id", _connection);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
            Disconnect();
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
            if (!await ConnectAsync())
                throw new InvalidOperationException("Could not establish a connection to the database.");

            await using var cmd = new NpgsqlCommand("SELECT * FROM ReceiptToProducts WHERE ReceiptID=@receiptId", _connection);
            cmd.Parameters.AddWithValue("receiptId", receiptId);

            await using var reader = await cmd.ExecuteReaderAsync();
            List<int> result = new List<int>();
            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ProductID"));
                result.Add(id);
            }

            Disconnect();
            return result;
        }
    }
}
