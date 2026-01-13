using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Console.WriteLine(
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
                    db.Customers.Add(newCustomer);
                    db.SaveChanges();
                }
            }
            return newCustomer;
        }

        public static void DeleteCustomer()
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
                        db.Remove(selectedCustomer);
                        db.SaveChanges();
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

            if (selectedCustomer != null)
            {
                Console.WriteLine("Edit - [1] Name, [2] Email, [3] Street, [4] City, [5] Country \n");
                switch (Console.ReadKey(true).KeyChar.ToString().ToUpper())
                {
                    case "1":
                        Console.Clear();
                        UpdateCustomerName(selectedCustomer);
                        break;
                    case "2":
                        Console.Clear();
                        UpdateCustomerEmail(selectedCustomer);
                        break;
                    case "3":
                        Console.Clear();
                        UpdateCustomerStreet(selectedCustomer);
                        break;
                    case "4":
                        Console.Clear();
                        UpdateCustomerCity(selectedCustomer);
                        break;
                    case "5":
                        Console.Clear();
                        UpdateCustomerCountry(selectedCustomer);
                        break;
                    default:
                        Helpers.MsgBadInputsAnyKey();
                        break;
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

                    db.Update(selectedCustomer);
                    db.SaveChanges();
                }
            }


        }

        public static void UpdateCustomerName(Customer customer)
        {
            Console.WriteLine("Update customer name...");
            Console.WriteLine($"Current name: {customer.Name}");
            Console.Write("Enter new name: ");
            string newName = Console.ReadLine();

            GenericServices.UpdateItemName(customer, newName); //Method handles input check.
            
        }

        public static void UpdateCustomerEmail(Customer customer)
        {
            Console.WriteLine("Update customer email...");
            Console.WriteLine($"Current email: {customer.Email}");
            Console.Write("Enter new email: ");
            string newEmail = Console.ReadLine();

            if (customer != null && !string.IsNullOrEmpty(newEmail))
            {
                using (var db = new WebShopContext())
                {
                    customer.Email = newEmail;
                    db.Update(customer);
                    db.SaveChanges();
                }
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }
        }

        public static void UpdateCustomerStreet(Customer customer)
        {
            Console.WriteLine("Update customer street...");
            Console.WriteLine($"Current street: {customer.Street}");
            Console.Write("Enter new steet name: ");
            string newStreetName = Console.ReadLine();

            if (customer != null && !string.IsNullOrEmpty(newStreetName))
            {
                using (var db = new WebShopContext())
                {
                    customer.Street = newStreetName;
                    db.Update(customer);
                    db.SaveChanges();
                }
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }
        }

        public static void UpdateCustomerCity(Customer customer)
        {
            Console.WriteLine("Update customer City...");
            Console.WriteLine($"Current city: {customer.City}");
            Console.Write("Enter new city name: ");
            string newCityName = Console.ReadLine();

            if (customer != null && !string.IsNullOrEmpty(newCityName))
            {
                using (var db = new WebShopContext())
                {
                    customer.City = newCityName;
                    db.Update(customer);
                    db.SaveChanges();
                }
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }
        }

        public static void UpdateCustomerCountry(Customer customer)
        {
            Console.WriteLine("Update customer country...");
            Console.WriteLine($"Current country: {customer.Country}");
            Console.Write("Enter new country name: ");
            string newCountryName = Console.ReadLine();

            if (customer != null && !string.IsNullOrEmpty(newCountryName))
            {
                using (var db = new WebShopContext())
                {
                    customer.Country = newCountryName;
                    db.Update(customer);
                    db.SaveChanges();
                }
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }
        }
        

    }
}
