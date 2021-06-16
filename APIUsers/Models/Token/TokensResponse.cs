using APIUsers.Models.Login;
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
    public class TokensResponse
    {

        public string access { get; set; }
        public string refresh { get; set; }

    }

    public class RefreshTokenModel
    {
        public string refresh { get; set; }
    }

    public class RefreshTokenResponse
    {
        public string access { get; set; }
    }
}
