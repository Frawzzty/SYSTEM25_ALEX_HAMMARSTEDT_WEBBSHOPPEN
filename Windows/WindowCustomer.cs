using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Modles;

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
                        $"ID:{customers[selectedCustomer].Id} - IsAdmin: {customers[selectedCustomer].IsAdmin}", 
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
            }
            else //If no customer is found - Create a new one
            {
                Console.WriteLine("No cusomter found. Please add one...");
                Customer newCustomer = CustomerServices.AddCustomer();
                
                return newCustomer.Id;
            }
            
            return customers[selectedCustomer].Id;
        }
    }
}
