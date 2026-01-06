using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;

namespace WebShop.Services
{
    internal class GenericsServices
    {
        public static bool UpdateItemName<T>(T item, string newName)
        {
            bool sucess = false;
            if(item is Product product)
            {
                product.Name = newName;
            }
            else if (item is Category category)
            {
                category.Name = newName;
            }

            if(item != null && !string.IsNullOrWhiteSpace(newName))
            using (var db = new WebShopContext())
            {
                db.Update(item);
                db.SaveChanges();
                sucess = true;
            }
            return sucess;
        }

        public static void PrintBaseClass<T>(List<T> items)
        {
            //Headers
            
            //Rows
        }

        public static bool DeleteItem<T>(T item)
        {
            bool success = false;

            using (var db = new WebShopContext()) 
            {
                try
                {
                    db.Remove(item);
                    db.SaveChanges(success);
                    success = true;
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine(ex.Message); 
                    Console.ReadKey(true); 
                }

            }
            return success;

        }
    }
}
