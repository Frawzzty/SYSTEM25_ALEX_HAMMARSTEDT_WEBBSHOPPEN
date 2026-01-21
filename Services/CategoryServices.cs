using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;

namespace WebShop.Services
{
    internal class CategoryServices
    {

        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using (var db = new WebShopContext())
            {
                categories = db.Categories.ToList();
            }

            return categories;
        }
        public static Category GetCategory(int id)
        {
            Category category = null;
            using (var db = new WebShopContext())
            {
                category = db.Categories.Where(c => c.Id == id).Include(c => c.Products).SingleOrDefault();
            }
            return category;
        }

        public static void PrintCategories(List<Category> categories)
        {
            Helpers.WriteLineInColor(ConsoleColor.Blue, "ID\tCategoryName");
            foreach (var category in categories) 
            {
                Console.WriteLine(category.Id + "\t" + category.Name); 
            }
        }

        public static void CreateCategory()
        {
            PrintCategories(GetAllCategories()); //Print all categories to get a refrence of what exists
            Console.WriteLine("\nCreate new category... Leave empty to exit");

            Console.Write("Enter name: ");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                using (var db = new WebShopContext())
                {
                    db.Categories.Add(new Category(input));
                    db.SaveChanges();
                }
            }
            else
                Helpers.MsgLeavingAnyKey();
        }


        public static void UpdateCategoryName()
        {
            PrintCategories(GetAllCategories());

            Console.WriteLine("\n[Leave inputs empty to exit] \nUpdate category name ");
            Console.Write("Enter ID: ");
            bool validId = int.TryParse(Console.ReadLine(), out int itemId) && itemId > 0;

            Console.Write("Enter new category name: ");
            string newName = Console.ReadLine();
            bool validName = !string.IsNullOrWhiteSpace(newName);

            if (validId && validName)
            {
                Category category = GetCategory(itemId);
                if (category != null)
                {
                    GenericServices.UpdateItemName(category, newName); //Returns true if manged to update
                }
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }
        }

        public static void DeleteCategory()
        {
            bool success = false;
            using (var db = new WebShopContext())
            {
                var allCategories = db.Categories.ToList();
                PrintCategories(allCategories);    //Give the user an overlook of what exists

                Console.WriteLine("\nDelete category... Leave empty to exit");
                Console.Write("Enter categoy ID: ");
                bool validId = int.TryParse(Console.ReadLine(), out int inputId) && inputId > 0;
                if (validId)
                {
                    Category selectedItem = allCategories.Where(c => c.Id == inputId).SingleOrDefault();

                    Console.WriteLine($"Confirm you want to DELETE {selectedItem.Name}: [Y] or [N]");
                    string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                    Console.WriteLine(key);
                    if(key == "Y")
                    {
                        db.Remove(selectedItem);
                        db.SaveChanges(); //WIll crash if deleting category with products
                        success = true;
                    }
                }

                if(!success)
                    Helpers.MsgLeavingAnyKey();
            }
        }




    }
}
