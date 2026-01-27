using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Models;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowLoginRegister
    {

        public static async Task<bool> AuthenticateAsync()
        {
            string email = "";
            string password = "";
            string message = "";

            bool isAuthenticated = false;

            bool isLopping = true;
            while (isLopping)
            {

                DrawLoginWindow(email, password, message);
                message = "";

                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                Console.Clear();
                switch (key)
                {
                    case "1": 
                        //Enter email
                        Console.Write("Enter email: ");
                        email = Console.ReadLine().ToLower();

                        break;

                    case "2": 
                        //Enter passowrd
                        Console.Write("Enter password: ");
                        password = Console.ReadLine().ToLower();

                        break;

                    case "3":
                        //Try to login
                        if (TryLogin(email, password))
                        {
                            isAuthenticated = true;
                            isLopping = false;
                        }
                        else
                        {
                            message = "Login failed"; //Shown in next 
                        }

                        break;

                    case "4": 
                        //Create user
                        message = 
                            await CustomerServices.RegisterCustomerAsync() == true ? 
                            "Registred successfully" : 
                            "Registration failed";
                        break;

                    case "9":  
                        //Exit
                        Environment.Exit(0);
                        break;
                }
                Console.Clear();
            }
            return isAuthenticated;


        }

        private static void DrawLoginWindow(string email, string password, string message)
        {
            List<string> windowText = new List<string>() {
                "",
                $"[1] Email: {email}",
                $"[2] Password: {new string('*', password.Length)}",
                $"",
                $"[3] Login",
                $"[4] Register",
                $"",
                $"[9] Exit",
                $"",
                message};

            var window = new Window("Login / Register", 1, 1, windowText);
            window.Draw(ConsoleColor.Yellow);
        }


        private static bool TryLogin(string email, string password)
        {

            bool logginSuccess = false;
            using (var db = new Connections.WebShopContext())
            {
                Stopwatch stopWatch = Stopwatch.StartNew();

                var customers = db.Customers.ToList();
                var customer = customers
                    .Where(c => c.Email == email && c.Password == password)
                    .SingleOrDefault();

                stopWatch.Stop();

                if (customer != null)
                {
                    logginSuccess = true;
                    Settings.SetCurrentCustomer(customer.Id);

                    MongoDbServices.AddUserActionAsync(new UserAction(customer.Id, UserActions.Logged_In, "Admin: " + customer.IsAdmin, stopWatch.ElapsedMilliseconds));
                }
            }

            return logginSuccess;

        }
    }
}
