using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Modles
{
    internal class Cart
    {
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public int UnitAmount { get; set; }

    }
}
