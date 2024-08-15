using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Backend.Data;
using Backend.Services;

class Program
{
    public static void Main(String[] args)
    {
        //TODO:
        //When making call from Api to Tesseract return 202 (in progress) while processing photo. Then when asked about data about this photo then return 200 Ok("In progress"), when done return Ok("data")
        //Verify user in API (can't access other user receipts)
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetSection("ConnectionString").Get<String>();
        if (connectionString == null)
        {
            Console.WriteLine("Connection string is not established");
            return;
        }
        PostgresConnection.ConnectionString = connectionString;
        var connection = new PostgresConnection();
        connection.ConnectAsync().GetAwaiter().GetResult();
        builder.Services.AddSingleton<IDatabaseHandler>(connection);
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<ProductService>();
        builder.Services.AddSingleton<ReceiptService>();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:5173") 
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigin");

        app.MapControllers();

        app.Run();
    }
}