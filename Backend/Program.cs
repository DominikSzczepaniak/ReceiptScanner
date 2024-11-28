using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Backend.Data;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        builder.Services.AddSingleton(new PostgresConnectionPool(connectionString, maxPoolSize: 10));
        builder.Services.AddSingleton<IDatabaseHandler, PostgresConnection>();
        builder.Services.AddSingleton<IReceiptProductConnectionService, ReceiptProductConnectionService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddSingleton<IReceiptService, ReceiptService>();
        builder.Services.AddSingleton<TokenProvider>();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetSection("JwtIssuer").Get<String>(),
                ValidateAudience = true,
                ValidAudience = builder.Configuration.GetSection("JwtAudience").Get<String>(),
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSecret").Get<String>()!)),
                ClockSkew = TimeSpan.Zero
            };
        });

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}