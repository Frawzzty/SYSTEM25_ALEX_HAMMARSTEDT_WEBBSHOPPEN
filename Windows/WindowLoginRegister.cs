using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Models;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowLoginRegister
    {



        public static async Task<bool> Authenticate()
        {
            string email = "";
            string password = "";
            string message = "";

            bool isAuthenticated = false;

            bool isActive = true;
            while (isActive)
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
                            isActive = false;
                        }
                        else
                        {
                            message = "Login failed"; //Shown in next 
                        }

                        break;

                    case "4": 
                        //Create user
                        message = await CustomerServices.RegisterCustomer() == true ? "Registred successfully" : "Registration failed";
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
                $"[2] Password: {password}",
                $"",
                $"[3] Login",
                $"[4] Register",
                $"",
                $"[9] Exit",
                "",
                message};

            var window = new Window("Login / Register", 1, 1, windowText);
            window.headerColor = ConsoleColor.Yellow;

            window.Draw();
        }


        private static bool TryLogin(string inputEmail, string inputPassowrd)
        {

            bool logginSuccess = false;
            using (var db = new WebShopContext())
            {
                var customers = db.Customers.ToList();
                var customer = customers.Where(c => c.Email == inputEmail).SingleOrDefault(); //Unique email per customer

                if (customer != null) 
                {
                    if(customer.Password == inputPassowrd)
                    {
                        logginSuccess = true;
                        Settings.SetCurrentCustomer(customer.Id);

                        MongoDbServices.AddUserAction(new UserAction(customer.Id, UserActions.Logged_In, "Admin: " + customer.IsAdmin));
                    }

                }
            }
            
            return logginSuccess;

        }

        

    }
}
