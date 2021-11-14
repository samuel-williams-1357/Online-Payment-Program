using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;
using System.Linq;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly TenmoApiClient tenmoApi = new TenmoApiClient();


        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!UserService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            Console.WriteLine();
            Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
            do
            {
                Console.WriteLine();
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests // Feature coming soon!");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks // Feature coming soon!");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            Console.WriteLine();
                            GetAccounBalance();
                            break;

                        case 2: // View Past Transfers
                            DisplayUserTransfers();
                            break;

                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 4: // Send TE Bucks
                            SendTeBucks();
                            break;

                        case 5: // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void GetAccounBalance()
        {
            Account balance = tenmoApi.GetAccountBalance();

            Console.WriteLine("Your currrent balance is: " + balance.account_Balance.ToString("C"));
        }

        private void DisplayUserTransfers()
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                Console.WriteLine($"Transfers");
                Console.WriteLine("ID      From/To         Amount");
                Console.WriteLine("----------------------------");

                List<Transfer> userTransfers = tenmoApi.GetUserTransfers();

                foreach (Transfer transfer in userTransfers)
                {
                    if (transfer.accountFromUserId == UserService.UserId) // The following if statements are used to properly display From/To
                    {
                        Console.WriteLine($"{transfer.transfer_Id}      {transfer.accountToUserName}      {transfer.amount.ToString("C")}");
                    }
                    else if (transfer.accountFromUserId != UserService.UserId)
                    {
                        Console.WriteLine($"{transfer.transfer_Id}      {transfer.accountFromUserName}      {transfer.amount.ToString("C")}");
                    }
                }
                Console.WriteLine("---------");
                Console.Write("Please enter transfer ID to view details (0 to cancel): ");
                int input;
                if (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (input == 0)
                {
                    return;
                }
                else
                {
                    foreach (Transfer transfer in userTransfers)
                    {
                        if (transfer.transfer_Id == input)
                        {
                            DisplayTransferDetails(transfer);
                        }
                    }
                }
            }
        }

        private void DisplayTransferDetails(Transfer transfers)
        {
            Console.WriteLine($"\nId: {transfers.transfer_Id}\n{transfers.accountFromUserName}\n{transfers.accountToUserName}\nType: Send" +
                $"\nStatus: Approved\nAmount: {transfers.amount:c}");
        }

        private void SendTeBucks()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("ID       Name");

            List<SafeUsersDisplays> allUsers = tenmoApi.GetAllUsers();

            foreach (SafeUsersDisplays user in allUsers)
            {
                if (user.userId != UserService.UserId)
                {
                    Console.WriteLine($"{user.userId}       {user.username}");
                }
            }
            Console.WriteLine("------");
            Console.Write("\nEnter the ID of the user you are sending to (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int userID))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }

            if (userID == 0)
            {
                return;
            }

            if (userID == UserService.UserId)
            {
                Console.WriteLine("Why are you trying to send money from yourself to yourself? No...just no.");
                return;
            }

            decimal amount;
            Account balanceCheck = tenmoApi.GetAccountBalance(); // Need to use Account object when pulling from the Api

            do
            {
                Console.Write("Enter amount: ");
                if (!decimal.TryParse(Console.ReadLine(), out amount))
                {
                    Console.WriteLine("Invalid input. Please enter only a number as a decimal, i.e 1.00");
                }

                if (amount > balanceCheck.account_Balance)
                {
                    Console.WriteLine($"You do not have enough funds to make this transfer. Your balance is {balanceCheck.account_Balance.ToString("C")}");
                }

            } while (amount > balanceCheck.account_Balance); //This do/while ensures that the user doesn't overwithdrawl their account


            Transfer transfer = new Transfer()
            {
                accountToUserId = userID,
                accountFromUserId = UserService.UserId,
                amount = amount
            };
            
            tenmoApi.PostUserTransfers(transfer);
            Console.WriteLine("\nYour transfer has been completed!");
        }
        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                authService.Login(loginUser);
            }
        }
    }
}
