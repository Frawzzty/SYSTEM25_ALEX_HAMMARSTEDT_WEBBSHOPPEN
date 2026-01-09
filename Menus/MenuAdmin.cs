using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Menus
{
    internal class MenuAdmin
    { 
        private static int cursorPosTop = 0;
        private static ConsoleColor menuColor = ConsoleColor.White;

        //MAIN BRANCH
        public static void MenuAdminMain()
        {
            string menuHeader = "Admin";
            bool loop = true;
            while (loop)
            {
                Helpers.DrawMenuWindow(new MenuAdminMain(), menuHeader);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuAdminMain)number)
                    {
                        case Enums.MenuAdminMain.Product:
                            MenuAdminProduct();
                            break;

                        case Enums.MenuAdminMain.Category:
                            MenuAdminCategory();

                            break;

                        case Enums.MenuAdminMain.Customers:
                            MenuAdminCustomer();

                            break;

                        case Enums.MenuAdminMain.Back:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }

        //SUB BRANCH
        public static void MenuAdminProduct()
        {
            string menuHeader = "Admin - Product";
            bool loop = true;
            while (loop)
            {
                Helpers.DrawMenuWindow(new MenuAdminProduct(), menuHeader);

                Console.WriteLine("\n");
                ProductServices.PrintProducts(ProductServices.GetAllProducts());

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuAdminProduct)number)
                    {
                        case Enums.MenuAdminProduct.Add_Product:
                            Console.WriteLine("Add product");
                            ProductServices.AddProduct();

                            break;

                        case Enums.MenuAdminProduct.Update_Product:
                            Console.WriteLine("Update product");
                            ProductServices.UpdateProduct();
                            break;

                        case Enums.MenuAdminProduct.Set_on_sale:
                            Console.WriteLine("Set / remove product on sale");
                            ProductServices.SetProductOnSale();
                            break;
                        case Enums.MenuAdminProduct.Delete_Product:
                            Console.WriteLine("Delete Product");
                            ProductServices.DeleteProduct();
                            break;
                            

                        case Enums.MenuAdminProduct.Back:
                            loop = false;
                            break;
                    }
                }
                Console.Clear();
            }
        }

        //SUB BRANCH
        public static void MenuAdminCategory()
        {
            string menuHeader = "Admin - Category";
            bool loop = true;
            while (loop)
            {
                Helpers.DrawMenuWindow(new MenuAdminCategory(), menuHeader);

                Console.WriteLine("\n");
                CategoryServices.PrintCategories(CategoryServices.GetAllCategories());

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuAdminCategory)number)
                    {
                        case Enums.MenuAdminCategory.Create_Category:
                            CategoryServices.CreateCategory();

                            break;

                        case Enums.MenuAdminCategory.Update_Category:
                            CategoryServices.UpdateCategoryName();

                            break;

                        case Enums.MenuAdminCategory.Delete_Category:
                            CategoryServices.DeleteCategory();
                            
                            break;

                        case Enums.MenuAdminCategory.Back:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }

        //SUB BRANCH
        public static void MenuAdminCustomer()
        {
            string menuHeader = "Admin - Customer";
            bool loop = true;
            while (loop)
            {
                Helpers.DrawMenuWindow(new MenuAdminCustomer(), menuHeader);

                Console.WriteLine("\n");
                CustomerServices.PrintCustomers(CustomerServices.GetAllCustomers());

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuAdminCustomer)number)
                    {
                        case Enums.MenuAdminCustomer.Add_Customer:
                            CustomerServices.AddCustomer();

                            break;

                        case Enums.MenuAdminCustomer.Update_Customer:
                            CustomerServices.UpdateCustomer();

                            break;

                        case Enums.MenuAdminCustomer.Order_History:
                            

                            break;

                        case Enums.MenuAdminCustomer.Set_Role:
                            CustomerServices.UpdateCusomterRole();

                            break;

                        case Enums.MenuAdminCustomer.Delete_Customer:
                            CustomerServices.DeleteCustomer();

                            break;

                        case Enums.MenuAdminCustomer.Back:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }


    }
}
