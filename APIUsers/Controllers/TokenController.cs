using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using APIUsers.Models.Login;
using APIUsers.Models.Token;
using APIUsers.Models.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtAuthenticationManager jwtAuthenticationManager;

        public TokenController(IJwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            var tokens = jwtAuthenticationManager.Authenticate(loginModel);

            if (tokens != null)
            {
                return StatusCode(200, tokens);
            }

            return StatusCode(400, jwtAuthenticationManager.AuthenticationErrorResponse());
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenModel refresh)
        {
            var refreshToken = jwtAuthenticationManager.GetRefreshToken(refresh);

            if (refreshToken != null)
            {
                return StatusCode(200, refreshToken);
            }

            return StatusCode(400, jwtAuthenticationManager.TokenErrorResponse());
            
        }
    }
}