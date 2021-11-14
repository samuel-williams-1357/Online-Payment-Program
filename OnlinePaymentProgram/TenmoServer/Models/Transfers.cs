using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfers
    {
        public int Transfer_Id { get; set; }

        public int Transfers_Type_Id { get; set; }

        public int Transfers_Status_Id { get; set; }

        public int Account_From { get; set; }

        public string AccountFromUserName { get; set; }

        public int AccountFromUserId { get; set; }

        public int Account_To { get; set; }

        public string AccountToUserName { get; set; }

        public int AccountToUserId { get; set; }

        public decimal Amount { get; set; }

    }
}
