using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Backend.Data;
using Backend.Services;

class Program
{
    public static void Main(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetSection("ConnectionString").Get<String>();
        PostgresConnection.ConnectionString = connectionString;
        builder.Services.AddSingleton<IDatabaseHandler>(sp =>
            new PostgresConnection());
        builder.Services.AddSingleton<UserService>();
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