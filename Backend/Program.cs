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

        string ConnectionString = "TODO";

        builder.Services.AddSingleton<IDatabaseHandler>(sp =>
            new PostgresConnection(builder.Configuration.GetConnectionString(ConnectionString)));
        builder.Services.AddSingleton<UserService>();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}