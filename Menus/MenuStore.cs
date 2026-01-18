using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuStore
    { 
        //MAIN BRANCH
        public static void MenuStoreMainDynamic()
        {
            int productsPerPage = 3;
            string menuHeader = "Store";
            bool isActive = true;

            while (isActive)
            {
                List<Category> categories = CategoryServices.GetAllCategories();

                //Create menu text for window
                string menuText = "";
                for (int i = 0; i < categories.Count; i++)
                {
                    menuText += $"[{i + 1}] {categories[i].Name} ";
                }
                //Adding hardcoded must exist values
                menuText += " - [F] Search - [9] Back";

                //Draw menu
                Helpers.DrawMenuText(menuHeader, menuText);
                
                //Menu inputs
                string input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                bool isValidCategory = int.TryParse(input, out int index) && index > 0 && index < 9 && index <= categories.Count();
                Console.Clear();
                if (isValidCategory)
                {
                    WindowBrowseProducts.ProductPageByCategory(categories[index -1].Id, productsPerPage);
                }
                //Search product by text
                else if (input == "F") 
                {
                    Console.Write("Enter search term: ");
                    string searchTerm = Console.ReadLine();
                    Console.Clear();
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        WindowBrowseProducts.ProductPageByList(ProductServices.GetProductsByString(searchTerm), productsPerPage);
                    }
                }
                //Go back
                else if(input == "9")
                {
                    isActive = false;
                }
                Console.Clear();
            }
        }

    }
}
