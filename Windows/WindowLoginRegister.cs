using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                DrawWindow(email, password, message);
                message = "";

                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                Console.Clear();
                switch (key)
                {
                    case "1": 
                        //Enter email
                        Console.Clear();
                        Console.Write("Enter email: ");
                        email = Console.ReadLine();

                        break;

                    case "2": 
                        //Enter passowrd
                        Console.Clear();
                        Console.Write("Enter password: ");
                        password = Console.ReadLine();

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
                            message = "Login failed";
                        }

                        break;

                    case "4": 
                        //Create user
                        message = await RegisterCustomer() == true ? "Registred successfully" : "Registration failed";
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

        private static void DrawWindow(string email, string password, string message)
        {
            var window = new Window("Login / Register", 1, 1, new List<string>() {
                $"[1] Email: {email}",
                $"[2] Password: {password}",
                $"[3] Login",
                $" ",
                $"[4] Register",
                $"[9] Exit",
            " ",
            message});
            window.headerColor = ConsoleColor.Green;
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
                        MongoDbServices.AddUserAction(new UserAction(customer.Id, UserActions.Logged_In, customer.Email));
                    }

                }
            }
            
            return logginSuccess;

        }

        private static async Task<bool> RegisterCustomer()
        {
            bool sucess = false;

            Console.Clear();
            Console.WriteLine("Register");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Street: ");
            string street = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("Country: ");
            string country = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            
            using (var db = new WebShopContext())
            {
                Customer customer = new Customer(name, street, city, country, email, password);

                try
                {
                    db.Customers.Add(customer);
                    await db.SaveChangesAsync();

                    sucess = true;
                    MongoDbServices.AddUserAction(new UserAction(customer.Id, UserActions.Customer_Added, customer.Email));

                    Console.WriteLine("Thanks for accepting marketing and cookikes :)");
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not Register user to DB... Check inputs and try again");
                    Console.WriteLine(ex.Message);
                }
            }

            return sucess;

        }

    }
}
