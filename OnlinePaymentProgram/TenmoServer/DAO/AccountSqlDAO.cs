using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountSqlDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        /// <summary>
        /// Pulls account balance of logged in user from database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Account GetAccountBalance(int userId)
        {
            Account account = new Account();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT balance, user_id, account_id FROM accounts WHERE user_id = @id", conn);

                cmd.Parameters.AddWithValue("@id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    account.Account_Balance = Convert.ToDecimal(reader["balance"]);
                    account.Account_Id = Convert.ToInt32(reader["account_id"]);
                    account.User_Id = Convert.ToInt32(reader["user_id"]);
                }

            }

            return account;
        }
    }
}
