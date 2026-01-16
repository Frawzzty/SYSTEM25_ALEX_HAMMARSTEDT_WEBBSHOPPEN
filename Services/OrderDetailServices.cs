using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;

namespace WebShop.Services
{
    internal class OrderDetailServices
    {

        public static void PrintOrderDetail(OrderDetail orderDetail)
        {
            Console.WriteLine("ID:          " + orderDetail.Id);
            Console.WriteLine("Order ID:    " + orderDetail.OrderId);
            Console.WriteLine("Product ID:  " + orderDetail.ProductId);
            Console.WriteLine("Price:       " + orderDetail.Price);
            Console.WriteLine("Unit Amount: " + orderDetail.UnitAmount);
        }
    }
}
