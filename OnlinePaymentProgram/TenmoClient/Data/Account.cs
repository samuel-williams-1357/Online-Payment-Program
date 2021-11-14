using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    /// <summary>
    /// This class is used to display the logged in user's account balance
    /// </summary>
    public class Account // Had issues deserializing objects when using typical C# Properties naming convention. The properties were not set because the client responses were not in JSON format
    {
        public int account_Id { get; set; }
        public int user_Id { get; set; }
        public decimal account_Balance { get; set; }

    }
}
