using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Modles
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ShippingMethod { get; set; }  //Enum?
        public string ShippingPrice { get; set; }   //Enum?
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime OrderDate { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}
