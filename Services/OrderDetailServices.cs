using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Services
{
    internal class OrderDetailServices
    {

        public static void PrintOrderDetail(OrderDetail orderDetail)
        {
            Console.WriteLine("ID:          " + orderDetail.Id);
            Console.WriteLine("Order ID:    " + orderDetail.OrderId);
            Console.WriteLine("Product ID:  " + orderDetail.ProductId);
            Console.WriteLine("Price:       " + orderDetail.SubTotal);
            Console.WriteLine("Unit Amount: " + orderDetail.UnitAmount);
        }

        public static List<OrderDetail> GetAllOrderDetails()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            using (var db = new Connections.WebShopContext())
            {
                //Including everyting possible
                orderDetails = db.OrderDetails
                    .Include(od => od.Product).ThenInclude(p => p.Category)
                    .Include(od => od.Order).ThenInclude(o => o.Customer)
                    .ToList();
            }

            return orderDetails;
        }
    }
}
