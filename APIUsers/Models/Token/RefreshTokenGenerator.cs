using System;
using System.Security.Cryptography;

namespace APIUsers.Models.Token
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        // Generate a refresh token
        public string GenerateToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
