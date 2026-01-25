using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class CartItem
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UnitAmount { get; set; }

        public CartItem() { }
        public CartItem(int customerId, int productId, int unitAmount)
        {
            CustomerId = customerId;
            ProductId = productId;
            UnitAmount = unitAmount;
        }

    }
}
