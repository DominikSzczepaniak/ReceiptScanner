﻿using Backend.Models;

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
        public Task<User> GetUserData(string username, string password);

        public Task RegisterUser(string username, string password);

        public Task DeleteUser(string username, string password);

        public Task<Receipt> GetReceiptData(int id);

        public Task<List<Receipt>> GetReceiptsForUser(int userId);

        public Task DeleteReceipt(int id);

        public Task AddReceipt(DateTime dateTime, string shopName, int ownerId);

        public Task AddProduct(string name, decimal price, decimal quantityWeight, string category, int ownerId);

        public Task<Product> GetProduct(int id);

        public Task<List<Product>> GetReceiptProducts(int receiptId);
        public Task<List<Receipt>> GetReceiptsBetweenDates(DateTime startDate, DateTime endDate, int ownerId);


        public Task DeleteProduct(int id);
        
        public Task DeleteReceiptProducts(int receiptId);
    }
}
