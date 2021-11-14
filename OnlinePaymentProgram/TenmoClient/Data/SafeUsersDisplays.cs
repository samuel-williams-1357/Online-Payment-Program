using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    /// <summary>
    /// The purpose of this class to safely display only non-sensitive info about users
    /// </summary>
    public class SafeUsersDisplays // Had issues deserializing objects when using typical C# Properties naming convention. The properties were not set because the client responses were not in JSON format
    {
        public int userId { get; set; }
        public string username { get; set; }

    }
}
