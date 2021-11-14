using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserDAO userDAO;

        public UsersController(IUserDAO _userDAO)
        {
            this.userDAO = _userDAO;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            List<User> result = userDAO.GetUsers();
            return Ok(result);
        }
    }

}
