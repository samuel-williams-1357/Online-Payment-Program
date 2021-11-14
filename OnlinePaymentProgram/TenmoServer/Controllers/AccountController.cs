using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSqlDAO accountDao;

        public AccountController(IAccountSqlDAO _accountDao)
        {
            this.accountDao = _accountDao;
        }
        /// <summary>
        /// Used to get the balance of a logged in user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult AccountBalance(int userId)
        {
            
            Account account = accountDao.GetAccountBalance(userId);

            int userIdCheck = int.Parse(this.User.FindFirst("sub").Value); 

            if (userIdCheck != account.User_Id)
            {
                return StatusCode(403, "You cannot view the sensitive information of other people's accounts");
            }
            return Ok(account);
        }
    }
}
