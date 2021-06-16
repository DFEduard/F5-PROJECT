using APIUsers.Models.Database;
using APIUsers.Models.Login;
using APIUsers.Models.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIUsers.Models.Token
{
    public interface IJwtAuthenticationManager
    {
        TokensResponse Authenticate(LoginModel loginModel);
        RefreshTokenResponse GetRefreshToken(RefreshTokenModel refreshTokenModel);
        Dictionary<string, object> TokenErrorResponse();
        Dictionary<string, object> AuthenticationErrorResponse();

    }

    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private IDatabaseManager dbManager;
        private readonly string jwtKey;
        private readonly string jwtIssuer;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly Dictionary<string, object> response;

        private const string tokenErrorMsg = "Given token not valid for any token type";
        private const string authenticationErrorMsg = "Invalid email or password";

        public JwtAuthenticationManager(string connectionString, string jwtKey, string jwtIssuer, IRefreshTokenGenerator refreshTokenGenerator)
        {
            dbManager = new DatabaseManager(connectionString);
            
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.jwtKey = jwtKey;
            this.jwtIssuer = jwtIssuer;

            response = new Dictionary<string, object>();
            response.Add("error", "");
        }


        public TokensResponse Authenticate(LoginModel loginModel)
        {
            if (!IsObjFieldsValidation(loginModel))
            {
                return null;
            }

            int userID = dbManager.ValidUserCredentials(loginModel.email,loginModel.password);

            if (userID != -1)
            {
                var tokens = GenerateTokenRefresh(loginModel.email);
                dbManager.RegisterRefreshToken(tokens.refresh, userID);
 
                return tokens;
            }

            response["error"] = authenticationErrorMsg;
            return null;
        }


        public RefreshTokenResponse GetRefreshToken(RefreshTokenModel refreshTokenModel)
        {
            if (!IsObjFieldsValidation(refreshTokenModel))
            {
                return null;
            }

            string email = dbManager.CheckRefreshToken(refreshTokenModel.refresh);
            string token = null;

            if (email != null)
            {
                token = GenerateToken(email, 1440);
                return new RefreshTokenResponse { access = token };
            };

            response["error"] = tokenErrorMsg;
            return null;

        }

        private TokensResponse GenerateTokenRefresh(string email)
        {
            var token = GenerateToken(email);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            return new TokensResponse {
                access=token,
                refresh=refreshToken
            };
        }

        
        private string GenerateToken(string email, int validMinutes=5)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenSecurity = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(validMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenSecurity);
        }

        public Dictionary<string, object> TokenErrorResponse()
        {
            return response;
        }

        public Dictionary<string, object> AuthenticationErrorResponse()
        {
            return response;
        }

        private bool IsObjFieldsValidation(object obj)
        {
            Dictionary<string, object> result = Validation.Fields(obj);

            if (result != null)
            {
                response["error"] = result;
                return false;
            }

            return true;
        }


    }
}
