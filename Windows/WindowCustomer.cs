using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
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
    internal class WindowCustomer
    {
        public static int SelectCustomer()
        {
            List<Customer> customers = CustomerServices.GetAllCustomers();
            int selectedCustomer = 0;
            int customerCount = customers.Count();

            
            if (customers.Count > 0)
            {
                bool isActive = true;
                while (isActive)
                {
                    Console.Clear();
                    //Draw controls
                    var controlsWindow = new Window($"Select Customer", 1, 1, new List<string> { 
                        $"Name: {customers[selectedCustomer].Name}",
                        $"ID: {customers[selectedCustomer].Id}",$"Admin: {customers[selectedCustomer].IsAdmin}", 
                        "[Q] Previous [E] Next  -  [9] Select " });
                    controlsWindow.Draw(ConsoleColor.Yellow);
                    

                    string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                    switch (key)
                    {
                        //Navigation keys
                        case "Q":
                            selectedCustomer = Math.Clamp(selectedCustomer -= 1, 0, customerCount -1);
                            break;

                        case "E":
                            selectedCustomer = Math.Clamp(selectedCustomer += 1, 0, customerCount - 1);
                            break;

                        case "9":
                            isActive = false;
                            break;
                    }
                }

                MongoDbServices.AddUserAction(new UserAction(customers[selectedCustomer].Id, UserActions.Logged_In));

                return customers[selectedCustomer].Id;

            }
            else //If no customer is found - Create a new one
            {
                Console.WriteLine("No cusomter found. Please add a new one...");
                Customer newCustomer = CustomerServices.AddCustomer();

                MongoDbServices.AddUserAction(new UserAction(newCustomer.Id, UserActions.Customer_Added));
                return newCustomer.Id;
            }
            
            
        }
    }
}
