using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Services
{
    internal class CategoryServices
    {

        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            using (var db = new Connections.WebShopContext())
            {
                categories = db.Categories.ToList();
            }

            return categories;
        }
        public static Category GetCategory(int id)
        {
            Category category = null;
            using (var db = new Connections.WebShopContext())
            {
                category = db.Categories.Where(c => c.Id == id).Include(c => c.Products).SingleOrDefault();
            }
            return category;
        }

        public static void PrintCategories(List<Category> categories)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Helpers.WriteLineInColor(ConsoleColor.White, "ID".PadRight(8) + "CategoryName".PadRight(13));
            Console.BackgroundColor = ConsoleColor.Black;
            int rowIndex = 0;
            foreach (var category in categories) 
            {
                if (rowIndex % 2 == 0)
                    Console.BackgroundColor = ConsoleColor.DarkGray;

                Console.WriteLine(category.Id.ToString().PadRight(8)+ category.Name.PadRight(13));

                Console.BackgroundColor = ConsoleColor.Black;
                rowIndex++;
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
                using (var db = new Connections.WebShopContext())
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
            using (var db = new Connections.WebShopContext())
            {
                var allCategories = db.Categories.ToList();
                PrintCategories(allCategories);    //Give the user an overlook of what exists

                Console.WriteLine("\nDelete category... Leave empty to exit");
                Console.Write("Enter categoy ID: ");
                bool validId = int.TryParse(Console.ReadLine(), out int inputId) && inputId > 0;
                if (validId)
                {
                    Category selectedItem = allCategories.Where(c => c.Id == inputId).SingleOrDefault();

                    Console.WriteLine($"\nConfirm you want to DELETE: [Y] or [N]");
                    string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                    if (key == "Y")
                    {
                        try
                        {
                            db.Remove(selectedItem);
                            db.SaveChanges();
                            success = true;
                        }
                        catch
                        {
                            Console.WriteLine("\nERROR: Invalid ID or selected category still contains prodcuts");
                        }
                    }

                    if (!success)
                    {
                        Helpers.MsgLeavingAnyKey();
                    }
                        
                }
            }

        }

    }
}
