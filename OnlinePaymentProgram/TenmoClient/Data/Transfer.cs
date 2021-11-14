using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    /// <summary>
    /// Represents a transfer made by a user
    /// </summary>
    public class Transfer // Had issues deserializing objects when using typical C# Properties naming convention. The properties were not set because the client responses were not in JSON format
    {
        public int transfer_Id { get; set; }
                   
        public int transfers_Type_Id { get; set; }
                   
        public int transfers_Status_Id { get; set; }

        public int account_From { get; set; }

        public string accountFromUserName { get; set; } // Placing this property in each transfer object simplifies display in the UserInterface

        public int accountFromUserId { get; set; }

        public int account_To { get; set; }

        public string accountToUserName { get; set; } // Placing this property in each transfer object simplifies display in the UserInterface

        public int accountToUserId { get; set; }

        public decimal amount { get; set; }

    }
}
