using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShop.Enums;
using WebShop.Models;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.DbServices
{
    internal class CustomerServices
    {
        public static List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (var db = new WebShopContext())
            {
                customers = db.Customers.Include(c => c.Orders).Include(c => c.CartItems).ToList();
            }
            return customers;
        }

        public static Customer GetCustomerById(int id)
        {
            Customer customers;
            using (var db = new WebShopContext())
            {
                //Include customer data
                customers = db.Customers.Include(c => c.Orders).Include(c => c.CartItems).Where(c => c.Id == id).SingleOrDefault();
            }
            return customers;
        }

        public static void PrintCustomers(List<Customer> customers)
        {
            if (customers != null) 
            {
                int cellpadding = 3; //Spacing between columns

                string headerId = "ID";
                string headerName = "Name";
                string headerEmail = "Email";
                string headerStreet = "Street";
                string headerCity = "City";
                string headerCountry = "Country";
                string headerIsAdmin = "Admin";

                int padId = Helpers.GetHeaderMaxPadding(headerId, customers.Max(item => item.Id.ToString().Length), 3);
                int padName = Helpers.GetHeaderMaxPadding(headerName, customers.Max(item => item.Name.Length), 3);
                int padEmail = Helpers.GetHeaderMaxPadding(headerEmail, customers.Max(item => item.Email.Length), 3);
                int padStreet = Helpers.GetHeaderMaxPadding(headerStreet, customers.Max(item => item.Street.Length), 3);
                int padCity = Helpers.GetHeaderMaxPadding(headerCity, customers.Max(item => item.City.Length), 3);
                int padCountry = Helpers.GetHeaderMaxPadding(headerCountry, customers.Max(item => item.Country.Length), 3);
                int padIsAdmin = Helpers.GetHeaderMaxPadding(headerIsAdmin, customers.Max(item => item.IsAdmin.ToString().Length), 3);

                Helpers.WriteLineInColor(ConsoleColor.Blue,
                    "  " + // Move everything 1 step to the right
                    headerId.PadRight(padId) +
                    headerName.PadRight(padName) +
                    headerEmail.PadRight(padEmail) +
                    headerStreet.PadRight(padStreet) +
                    headerCity.PadRight(padCity) +
                    headerCountry.PadRight(padCountry) +
                    headerIsAdmin.PadRight(padIsAdmin)
                );

                foreach (var customer in customers)
                {
                    Console.WriteLine("  " + //Move everything 1 step to the right
                        customer.Id.ToString().PadRight(padId) +
                        customer.Name.PadRight(padName) +
                        customer.Email.PadRight(padEmail) +
                        customer.Street.PadRight(padStreet) +
                        customer.City.PadRight(padCity) +
                        customer.Country.PadRight(padCountry) +
                        customer.IsAdmin.ToString().PadRight(padIsAdmin));
                }
            }
            else
            {
                Console.WriteLine("List is empty...");
            }

        }

        /// <summary>
        /// Lets user create customer with inputs
        /// </summary>
        /// <returns>the customer created</returns>
        public static Customer AddCustomer()
        {
            Console.WriteLine("Add new Customer...");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Street: ");
            string street = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();
            Console.Write("Country: ");
            string country = Console.ReadLine();

            Customer newCustomer = new Customer(name, email, street, city, country);

            if (newCustomer != null) 
            {
                using (var db = new WebShopContext())
                {
                    try
                    {
                        db.Customers.Add(newCustomer);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not add new customer" + "\n");
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            return newCustomer;
        }

        public static async Task<bool> RegisterCustomer()
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

                if (validateCustomerDetails(customer)) //Checks details are not empty
                {
                    try
                    {
                        db.Customers.Add(customer);
                        await db.SaveChangesAsync();

                        sucess = true;
                        MongoDbServices.AddUserAction(new UserAction(customer.Id, UserActions.Customer_Added, customer.Email));


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not Register user Check inputs and try again");
                        Console.WriteLine(ex.Message);
                    }
                }
               
            }

            return sucess;

        }

        public static async void DeleteCustomer()
        {
            List<Customer> customers = GetAllCustomers();
            PrintCustomers(customers);

            Console.Write("\nDelete Customer - Enter ID: ");
            bool isValidInputID = int.TryParse(Console.ReadLine(), out int userId) && userId > 0;
            Customer selectedCustomer = customers.Where(c => c.Id == userId).SingleOrDefault();

            if (selectedCustomer != null)
            {
                Console.WriteLine($"\nAre you sure you want to delete: {selectedCustomer.Name}");
                Console.WriteLine("Press [Y] to DELETE or Cancle [Any key]");
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                if (key == "Y")
                {
                    using (var db = new WebShopContext())
                    {
                        try
                        {
                            db.Remove(selectedCustomer);
                            await db.SaveChangesAsync();
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Could not delete  customer" + "\n");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Helpers.MsgLeavingAnyKey();
                }
            }
            else
            {
                Console.WriteLine("User not found. Any key to constinue...");
                Console.ReadKey(true);
            }
        }

        public static void UpdateCustomer()
        {
            List<Customer> customers = GetAllCustomers();
            PrintCustomers(customers);

            Console.Write("\nChoose customer - Enter ID: ");
            bool isValidInputID = int.TryParse(Console.ReadLine(), out int userId) && userId > 0;
            Customer selectedCustomer = customers.Where(c => c.Id == userId).SingleOrDefault();

            Console.Clear();
            if (selectedCustomer != null)
            {
                bool isActive = true;
                while (isActive)
                {
                    //Print menu and selected customer
                    Helpers.DrawMenuEnum(new Enums.UpdateCustomer(), "Update Customer");
                    
                    PrintCustomers(new List<Customer> { selectedCustomer });
                    Console.WriteLine("\n\n");

                    //Handle input
                    string input = Console.ReadKey(true).KeyChar.ToString();
                    if(int.TryParse(input, out int number) && number <= Enum.GetNames(typeof(Enums.UpdateCustomer)).Length) //Check number is less than enum menu length
                    {
                        UpdateCustomerer(selectedCustomer, (Enums.UpdateCustomer)number);
                    }
                    else
                    {
                        isActive = false;
                    }
                    
                    Console.Clear();
                }
            }
        }


        public static void UpdateCustomerer(Customer existingCustomer, Enums.UpdateCustomer enumOption)
        {
            Console.Write("Enter new" + enumOption.ToString().Replace("Update_", " ") + ": ");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                using (var db = new WebShopContext())
                {
                    try
                    {
                        UserAction userAction = new UserAction() { CustomerId = Settings.GetCurrentCustomerId(), Action = Enums.UserActions.Product.ToString() };
                        switch (enumOption)
                        {
                            case Enums.UpdateCustomer.Update_Name:
                                userAction.Details = $"{enumOption}: {existingCustomer.Name} : {input}";
                                existingCustomer.Name = input;
                                break;

                            case Enums.UpdateCustomer.Update_street:
                                userAction.Details = $"{enumOption}: {existingCustomer.Street} : {input}";
                                existingCustomer.Street = input;
                                break;

                            case Enums.UpdateCustomer.Update_city:
                                userAction.Details = $"{enumOption}: {existingCustomer.City} : {input}";
                                existingCustomer.City = input;
                                break;

                            case Enums.UpdateCustomer.Update_Country:
                                userAction.Details = $"{enumOption}: {existingCustomer.Country} : {input}";
                                existingCustomer.Country = input;
                                break;

                            case Enums.UpdateCustomer.Update_Email:
                                userAction.Details = $"{enumOption}: {existingCustomer.Email} : {input}";
                                existingCustomer.Email = input;
                                break;

                            case Enums.UpdateCustomer.Update_Password:
                                userAction.Details = $"Update password";
                                existingCustomer.Password = input;
                                break;
                        }

                        db.Update(existingCustomer);
                        db.SaveChanges();

                        MongoDbServices.AddUserAction(userAction);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not update " + enumOption.ToString().Replace("Update_", " ") + "\n");
                        Console.WriteLine(ex.Message);
                        Console.ReadKey(true);
                    }
                }
            }
        }

        public static void UpdateCusomterRole()
        {
            List<Customer> customers = GetAllCustomers();
            PrintCustomers(customers);

            Console.WriteLine("\nSet Role");
            Console.Write("Enter user ID: ");
            bool isValidInputID = int.TryParse(Console.ReadLine(), out int userId) && userId > 0;
            Customer selectedCustomer = customers.Where(c => c.Id == userId).SingleOrDefault();

            if (selectedCustomer != null)
            {
                using (var db = new WebShopContext())
                {
                    Console.WriteLine($"\nCurrent Admin status: {selectedCustomer.IsAdmin}");
                    Console.Write("[Y] to make Admin Or Cancle [Any key]");
                    if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Y")
                    {
                        selectedCustomer.IsAdmin = true;
                    }
                    else
                    {
                        selectedCustomer.IsAdmin = false;
                    }

                    try
                    {
                        db.Update(selectedCustomer);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not update customer role" + "\n");
                        Console.WriteLine(ex.Message);
                    }
;
                }
            }
        }

        public static bool validateCustomerDetails(Customer customer)
        {
            bool isValidCustomerDetails = true;

            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.Name)        ?   true : false;
            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.Email)       ?   true : false;
            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.Street)      ?   true : false;
            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.City)        ?   true : false;
            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.Country)     ?   true : false;
            isValidCustomerDetails = !string.IsNullOrWhiteSpace(customer.Password)    ?   true : false;

            return isValidCustomerDetails;

        }
    }
}
