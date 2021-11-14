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
    public class TransferController : ControllerBase
    {
        private readonly ITransferSqlDAO transferDAO;

        public TransferController(ITransferSqlDAO _transferDAO)
        {
            this.transferDAO = _transferDAO;
        }
        /// <summary>
        /// Completes a transfer and uploads it to the database
        /// </summary>
        /// <param name="newTransfer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult MakeTransfer(Transfers newTransfer)
        {
            Transfers result = transferDAO.MakeTransfer(newTransfer);

            int userId = int.Parse(this.User.FindFirst("sub").Value);

            if (userId != newTransfer.AccountFromUserId)
            {
                return StatusCode(403, "You cannot transfer money from other people's account to your own account");
            }
            return StatusCode(201, result);
        }
        /// <summary>
        /// Returns all transfers associated with logged in user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Authorize]
        public IActionResult ViewTransfers(int userId)
        {
            List<Transfers> result = transferDAO.ViewTransfers(userId);
            return Ok(result);
        }
    }
}
