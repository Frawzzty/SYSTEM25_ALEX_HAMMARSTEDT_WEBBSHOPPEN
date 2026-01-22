using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Modles
{
    internal class Customer

    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsAdmin { get; set; } = false; //Default false
        public string Password { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Customer() {}
        public Customer(string name, string email, string street, string city, string country)
        {
            Name =      Helpers.FirtUpperCaseRestLower(name);
            Email =     email.ToLower();
            Street =    Helpers.FirtUpperCaseRestLower(street);
            City =      Helpers.FirtUpperCaseRestLower(city);
            Country =   Helpers.FirtUpperCaseRestLower(country);
        }

        public Customer(string name, string street, string city, string country, string email, string password)
        {
            Name = Helpers.FirtUpperCaseRestLower(name);
            Street = Helpers.FirtUpperCaseRestLower(street);
            City = Helpers.FirtUpperCaseRestLower(city);
            Country = Helpers.FirtUpperCaseRestLower(country);

            Email = email.ToLower();
            Password = password; 
        }
    }
}
