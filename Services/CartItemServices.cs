using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.DbServices
{
    internal class CartItemServices
    {
        public static void AddCartItem(int productId, int customerId)
        {
            CartItem cartItem = null;

            // Check if cart item exists in customer
            Customer customer = CustomerServices.GetCustomerById(customerId);
            foreach (var item in customer.CartItems)
            {
                if (productId == item.ProductId)
                {
                    cartItem = item;
                    break;
                }
            }

            using (var db = new WebShopContext())
            {
                if (cartItem != null)
                {
                    // + 1 to existing cartitem
                    cartItem.UnitAmount++;
                    db.Update(cartItem);
                }
                else
                {
                    // create new cartitem
                    cartItem = new CartItem(customerId, productId, 1);
                    db.Add(cartItem);
                }
                db.SaveChanges();
            }
        }

        //GetCartItems
        public static List<CartItem> GetCartItemsByCustomerId(int customerId)
        {
            List<CartItem> cartItems;
            using (var db = new WebShopContext()) 
            {
                cartItems = db.CartItems.Include(ci => ci.Customer).Include(ci => ci.Product).Where(ci => ci.CustomerId == customerId).ToList();
            }
            return cartItems;
        }


        public static void UpdateCartItem(CartItem cartItem, int value)
        {
            if (cartItem != null) 
            {
                cartItem.UnitAmount += value;

                using (var db = new WebShopContext())
                {
                    if (cartItem.UnitAmount > 0)
                    {
                        db.Update(cartItem);
                    }
                    else
                    {
                        db.Remove(cartItem);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static void PrintCartItems(int customerId)
        {
            List<CartItem> cartItems = GetCartItemsByCustomerId(customerId);

            foreach (var cartItem in cartItems) 
            {
                Console.WriteLine("Item ID: " + cartItem.Id + " " + cartItem.Product.Name);
            }
        }
    }
}
