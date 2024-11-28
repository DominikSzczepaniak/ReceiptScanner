using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Data;

public class LoginUser(IDatabaseHandler db, PasswordHasher<User> passwordHasher, TokenProvider tokenProvider)
{
    public sealed record Request(string Username, string Password);

    public async Task<string> Handle(Request request)
    {
        User? user = await db.GetUserData(request.Username, request.Password);
        
        if (user is null)
        {
            throw new Exception("User not found");
        }
        
        bool verified = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success;
        
        if (!verified)
        {
            throw new Exception("Invalid password");
        }
        
        var token = tokenProvider.Create(user);
        
        return token;
    }
}