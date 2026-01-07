using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;

namespace WebShop.Services
{
    internal class ProductServices
    {
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (var db = new WebShopContext())
            {
                products = db.Products.ToList();
            }

            return products;
        }

        public static Product GetProductById(int id)
        {
            Product product = null;
            using (var db = new WebShopContext())
            {
                product = db.Products.Where(p => p.Id == id).SingleOrDefault();
            }
            return product;
        }

        public static void PrintProducts(List<Product> products)
        {
            int cellpadding = 3;
            if(!products.IsNullOrEmpty())
            {   //Set paddings
                int padId =             products.Max(item => item.Id.ToString().Length) + cellpadding;
                int padName =           products.Max(item => item.Name.Length) + cellpadding;
                int padDescription =    products.Max(item => item.Description.Length) + cellpadding;
                int padCategoryId =     10 + cellpadding;
                int padSupplierName =   products.Max(item => item.SupplierName.Length) + cellpadding;
                int padUnitPrice =      9 + cellpadding; //Manual as column name is usually longer than value
                int padUnitSalePrice =  13 + cellpadding; //Manual as column name is usually longer than value
                int padOnSale =         6 + cellpadding; //Manual as column name is usually longer than value
                int padStockAmount =    9 + cellpadding; //Manual as column name is usually longer than value

                //Headers
                Helpers.WriteLineInColor(ConsoleColor.Blue,
                    "ID".PadRight(padId) +
                    "Name".PadRight(padName) +
                    "Description".PadRight(padDescription) +
                    "CategoryID".PadRight(padCategoryId) +
                    "SupplierName".PadRight(padSupplierName) +
                    "UnitPrice".PadRight(padUnitPrice) +
                    "UnitSalePrice".PadRight(padUnitSalePrice) +
                    "OnSale".PadRight(padOnSale) +
                    "StockAmount".PadRight(padStockAmount)
                    );

                //Rows
                foreach (var product in products)
                {
                    Console.WriteLine(
                        product.Id.ToString().PadRight(padId) + 
                        product.Name.PadRight(padName) +
                        product.Description.PadRight(padDescription) +
                        product.CategoryId.ToString().PadRight(padCategoryId) +
                        product.SupplierName.PadRight(padSupplierName) +
                        product.UnitPrice.ToString().PadRight(padUnitPrice) +
                        product.UnitSalePrice.ToString().PadRight(padUnitSalePrice) +
                        product.OnSale.ToString().PadRight(padOnSale) +
                        product.StockAmount.ToString().PadRight(padStockAmount)
                        );
                }
            }
            else
            {
                Console.WriteLine("List is empty...");
            }

        }

        public static void AddProduct()
        {
            using (var db = new WebShopContext())
            {
                PrintProducts(db.Products.ToList());
                Console.WriteLine();

                Console.WriteLine("Add new product");
                //Get Inputs
                Console.Write("Enter name: ");          
                string name = Console.ReadLine();

                Console.Write("Enter description: ");
                string description = Console.ReadLine();

                CategoryServices.PrintCategories(CategoryServices.GetAllCategories());
                Console.Write("Enter category ID: ");
                bool isValidInputID = int.TryParse(Console.ReadLine(), out int categoryId) && categoryId > 0;

                Console.Write("Enter Supplier name: ");
                string supplierName = Console.ReadLine();

                Console.Write("Enter unit price [00,00]: ");
                bool isValidUnitPrice = decimal.TryParse(Console.ReadLine().Replace('.',','), out decimal unitPrice) && unitPrice > 0;

                Console.Write("Enter stock amount: ");
                bool isValidInputStockAmount = int.TryParse(Console.ReadLine(), out int stockAmount) && stockAmount > 0;
                
                if(
                    !string.IsNullOrEmpty(name) && 
                    !string.IsNullOrEmpty(description) && 
                    isValidInputID && 
                    !string.IsNullOrEmpty(supplierName) && 
                    isValidUnitPrice && 
                    isValidInputStockAmount)
                {
                    Console.Write("\n[Y] to add prodcut Or cancle [Any key]\n");
                    if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Y")
                    {
                        try
                        {
                            Product newProduct = new Product(name, description, categoryId, supplierName, unitPrice, stockAmount);
                            db.Products.Add(newProduct);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.InnerException);
                            Console.ReadKey(true);
                        }
                    }
                    else { Helpers.MessageLeavingAnyKey();}
                }
                else { Helpers.MessageBadInputsAnyKey();}
            }
        }

        public static void UpdateProductName()
        {
            PrintProducts(GetAllProducts());

            Console.WriteLine("\n[Leave inputs empty to exit] \nUpdate product name ");
            Console.Write("Enter ID: ");
            bool validId = int.TryParse(Console.ReadLine(), out int itemId) && itemId > 0;

            Console.Write("Enter new product name: ");
            string newName = Console.ReadLine();
            bool validName = !string.IsNullOrWhiteSpace(newName);

            if (validId && validName)
            {
                Product product = GetProductById(itemId);
                if (product != null)
                {
                    GenericServices.UpdateItemName(product, newName); //Returns true if manged to update
                }
            }
            else
            {
                Helpers.MessageBadInputsAnyKey();
            }
        }

        public static void UpdateProduct()
        {
            PrintProducts(GetAllProducts());
            using (var db = new WebShopContext())
            {
                Console.WriteLine("\n[Leave inputs empty to exit] \nUpdate product");
                Console.Write("Enter ID: ");
                bool validId = int.TryParse(Console.ReadLine(), out int itemId) && itemId > 0;

                Product product = db.Products.Where(p => p.Id == itemId).SingleOrDefault();

                if (product != null) 
                {
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter description: ");
                    string description = Console.ReadLine();

                    CategoryServices.PrintCategories(CategoryServices.GetAllCategories());
                    Console.Write("Enter category ID: ");
                    bool isValidInputID = int.TryParse(Console.ReadLine(), out int categoryId) && categoryId > 0;

                    Console.Write("Enter Supplier name: ");
                    string supplierName = Console.ReadLine();

                    Console.Write("Enter unit price [00,00]: ");
                    bool isValidUnitPrice = decimal.TryParse(Console.ReadLine().Replace('.', ','), out decimal unitPrice) && unitPrice > 0;

                    Console.Write("Enter stock amount: ");
                    bool isValidInputStockAmount = int.TryParse(Console.ReadLine(), out int stockAmount) && stockAmount > 0;

                    //Check inputs are valid
                    if (
                     !string.IsNullOrEmpty(name) &&
                     !string.IsNullOrEmpty(description) &&
                     isValidInputID &&
                     !string.IsNullOrEmpty(supplierName) &&
                     isValidUnitPrice &&
                     isValidInputStockAmount)
                    {
                        Console.Write("\n[Y] to update prodcut Or cancle [Any key]\n");
                        if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Y")
                        {
                            try
                            {
                                product.Name = name; 
                                product.Description = description;
                                product.CategoryId = categoryId;
                                product.SupplierName = supplierName;
                                product.UnitPrice = unitPrice;
                                product.StockAmount = stockAmount;

                                db.Update(product);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.ReadKey(true);
                            }
                        }
                        else { Helpers.MessageLeavingAnyKey(); }
                    }
                    else { Helpers.MessageBadInputsAnyKey(); }
                }
                else
                {
                    Console.WriteLine("Product ID returned NULL");
                    Helpers.MessageLeavingAnyKey();
                }
            }
        }
        public static void SetProductOnSale()
        {
            PrintProducts(GetAllProducts());

            Console.WriteLine("\n[Leave inputs empty to exit] \nSet / remove product on sale");
            Console.Write("Enter ID: ");
            bool validId = int.TryParse(Console.ReadLine(), out int itemId) && itemId > 0; //TODO Make inputs Methods?
            Product product;
            if (validId)
            {
                product = GetProductById(itemId);

                if (product != null)
                {
                    Console.WriteLine("Is product on sale Y / N");
                    string keyOnSale = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                    bool isValidUnitSalePrice = false;
                    if (keyOnSale == "Y")
                    {
                        product.OnSale = true;

                        Console.Write("Enter unit sale price [00,00]: ");
                        isValidUnitSalePrice = decimal.TryParse(Console.ReadLine().Replace('.', ','), out decimal unitSalePrice) && unitSalePrice > 0;
                        if (isValidUnitSalePrice)
                            product.UnitSalePrice = unitSalePrice;
                        
                    }
                    else if (keyOnSale == "N")
                    {
                        product.OnSale = false;
                        product.UnitSalePrice = 0;
                    }

                    if(keyOnSale == "Y" || keyOnSale == "N") //Update if yes or no.
                    {
                        using (var db = new WebShopContext())
                        {
                            db.Update(product);
                            db.SaveChanges();
                        }
                    }
 
                    else { Helpers.MessageLeavingAnyKey(); }

                }
                else { Console.WriteLine("Products returned NULL"); } //TODO Add readkey? / HELPER MESSAGE
            }
            else { Console.WriteLine("Invalid ID / ID input"); } //TODO Add readkey? / HELPER MESSAGE
        }
            
        public static void DeleteProduct()
        {
            List<Product> products = new List<Product>();
            products = GetAllProducts();
            PrintProducts(products);

            Console.WriteLine("\n[Leave input empty to cancle] \nDelete product");
            Console.Write("Enter ID: ");
            bool validId = int.TryParse(Console.ReadLine(), out int itemId) && itemId > 0; //TODO Make inputs Methods?
            if (validId) 
            {
                Product product = products.Where(p => p.Id == itemId).SingleOrDefault();
                if (product != null) 
                {
                    GenericServices.DeleteItem(product);
                }
            }
        }
    }
}
