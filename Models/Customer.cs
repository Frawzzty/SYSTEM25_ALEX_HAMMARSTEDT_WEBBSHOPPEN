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
        public bool IsAdmin { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Customer(string name, string email, string street, string city, string country)
        {
            Name = name;
            Email = email;
            Street = street;
            City = city;
            Country = country;
            IsAdmin = false; //Default off. 
        }
    }
}
