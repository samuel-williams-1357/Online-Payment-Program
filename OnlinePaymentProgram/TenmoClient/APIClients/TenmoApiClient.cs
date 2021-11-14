using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using RestSharp.Authenticators;
using System.Linq;

namespace TenmoClient.APIClients
{
    public class TenmoApiClient
    {
        private const string API_URL = "https://localhost:44315/";
        private readonly RestClient client = new RestClient();

        public Account GetAccountBalance()
        {
            RestRequest request = new RestRequest($"{API_URL}account/{UserService.UserId}");
            request.AddHeader("Authorization", $"bearer {UserService.Token}");

            IRestResponse<Account> response = client.Get<Account>(request);

            if(response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with the server");
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Could not get Account Balance. Error status code " + response.StatusCode);
            }

            

            return response.Data;

        }

        public List<SafeUsersDisplays> GetAllUsers()
        {
            RestRequest request = new RestRequest($"{API_URL}users");
            request.AddHeader("Authorization", $"bearer {UserService.Token}");

            IRestResponse<List<SafeUsersDisplays>> response = client.Get<List<SafeUsersDisplays>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with the server");
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Could not get Users. Error status code " + response.StatusCode);
            }
            return response.Data;
        }

        public List<Transfer> GetUserTransfers()
        {
            RestRequest request = new RestRequest($"{API_URL}transfer/{UserService.UserId}");
            request.AddHeader("Authorization", $"bearer {UserService.Token}");

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with the server");
                return new List<Transfer>();
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Could not get User ID. Error status code " + response.StatusCode);
                return new List<Transfer>();
            }

            return response.Data;
        }

        public Transfer PostUserTransfers(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}transfer");
            request.AddHeader("Authorization", $"bearer {UserService.Token}");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Could not communicate with the server");
                return null;
            }

            if (!response.IsSuccessful)
            {
                Console.WriteLine("Could not make transfer. Error status code " + response.StatusCode);
                return null;
            }

            return response.Data;

        }

    }
}
