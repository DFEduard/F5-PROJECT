using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIUsers.Models.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data.SqlClient;
using static APIUsers.Models.User.Users;

namespace APIUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo userRepo;

        public UsersController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpPost]
        public IActionResult Post([FromBody] Users user)
        {

            if (userRepo.Create(user))
            {
                return StatusCode(201, user.ReadOnly(true));
            }

            return StatusCode(400, userRepo.UserErrorResponse());
            
        }

        // GET api/users
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = userRepo.Get(id);
            if (user != null)
            {
                return Ok(user.ReadOnly());
            }

            return StatusCode(404, Utils.NotFoundResponse());
           

        }

        // GET api/users/{id}
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            var users = userRepo.Get();
            if (users != null)
            {
                return StatusCode(200,users);
            }

            return StatusCode(404, userRepo.UserErrorResponse());
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserEdit user)
        {
            var _user = userRepo.Put(id, user);
            if (_user != null)
            {
                return StatusCode(200, _user.ReadOnly());
            }

            return StatusCode(400, userRepo.UserErrorResponse());
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = userRepo.Delete(id);
            if (response)
            {
                return StatusCode(200);
            }

            return StatusCode(400, userRepo.UserErrorResponse());
        }
    }
}