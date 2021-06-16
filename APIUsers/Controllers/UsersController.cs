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
                List<Object> usersList = new List<Object>();
                foreach (var user in users)
                {
                    usersList.Add(user.ReadOnly());
                }
                return Ok(usersList);
            }

            return StatusCode(404, Utils.NotFoundResponse());
            
        }
    }
}