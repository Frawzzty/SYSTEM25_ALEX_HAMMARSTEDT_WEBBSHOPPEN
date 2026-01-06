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

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders{ get; set; }
    }
}
