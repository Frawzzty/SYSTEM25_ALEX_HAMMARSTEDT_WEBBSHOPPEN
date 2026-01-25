using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebShop.Connections;
using WebShop.Migrations;
using WebShop.Models;
using WebShop.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.Services
{
    internal class ProductServices
    {
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (var db = new Connections.WebShopContext())
            {
                products = db.Products.ToList();
            }

            return products;
        }


        public static Product GetProductById(int id)
        {
            Product product = null;
            using (var db = new Connections.WebShopContext())
            {
                product = db.Products.Where(p => p.Id == id).SingleOrDefault();
            }
            return product;
        }


        public static List<Product> GetProductsByCategory(string categoryName)
        {
            List<Product> products = new List<Product>();
            using (var db = new Connections.WebShopContext())
            {
                products = db.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName).ToList(); //Use .Include so we are able to access the category props
            }
            return products;
            
        }


        public static List<Product> GetProductsByCategory(int categoryId)
        {
            List<Product> products = new List<Product>();
            using (var db = new Connections.WebShopContext())
            {
                products = db.Products
                    .Include(p => p.Category) //Use .Include() to get access to Navigation property
                    .Where(p => 
                        p.Category.Id == categoryId)
                    .ToList();
            }
            return products;

        }


        public static List<Product> GetProductsOnSale()
        {
            List<Product> products = new List<Product>();
            using (var db = new Connections.WebShopContext())
            {
                products = db.Products //Use .Include() to get access to Navigation property
                    .Include(p => p.Category)
                    .Where(p => 
                        p.IsOnSale == true)
                    .ToList(); 
            }
            return products;
        }


        public static List<Product> GetProductsByString(string searchTerm)
        {
            List<Product> products = new List<Product>();
            using (var db = new Connections.WebShopContext())
            {
                products = db.Products
                    .Include(p => p.Category) //Use .Include() to get access to Navigation property
                    .Where(p => 
                        p.Name.Contains(searchTerm) ||
                        p.Description.Contains(searchTerm) ||
                        p.SupplierName.Contains(searchTerm) ||
                        p.Category.Name.Contains(searchTerm))
                    .ToList(); 
                
            }
            return products;
        }


        public static void PrintProducts(List<Product> products)
        {
            if(!products.IsNullOrEmpty())
            {
                int cellpadding = 3; //Spacing between columns

                string headerId =           "ID";
                string headerName =         "Name";
                string headerDescription =  "Description";
                string headerCategoryId =   "CategoryID";
                string headerSupplierName = "SupplierName";
                string headerUnitPrice =    "UnitPrice";
                string headerUnitSalePrice ="UnitSalePrice";
                string headerOnSale =       "OnSale";
                string headerStockAmount =  "StockAmount";
                //Set paddings
                
                int padId =             Helpers.GetHeaderMaxPadding(headerId, products.Max(item => item.Id.ToString().Length), cellpadding);
                int padName =           Helpers.GetHeaderMaxPadding(headerName, products.Max(item => item.Name.Length), cellpadding);
                int padDescription =    Helpers.GetHeaderMaxPadding(headerDescription, products.Max(item => item.Description.Length), cellpadding);
                int padCategoryId =     Helpers.GetHeaderMaxPadding(headerCategoryId, products.Max(item => item.CategoryId.ToString().Length), cellpadding);
                int padSupplierName =   Helpers.GetHeaderMaxPadding(headerSupplierName, products.Max(item => item.SupplierName.Length), cellpadding);
                int padUnitPrice =      Helpers.GetHeaderMaxPadding(headerUnitPrice, products.Max(item => item.UnitPrice.ToString().Length), cellpadding);
                int padUnitSalePrice =  Helpers.GetHeaderMaxPadding(headerUnitSalePrice, products.Max(item => item.UnitSalePrice.ToString().Length), cellpadding);
                int padOnSale =         Helpers.GetHeaderMaxPadding(headerOnSale, products.Max(item => item.IsOnSale.ToString().Length), cellpadding);
                int padStockAmount =    Helpers.GetHeaderMaxPadding(headerStockAmount, products.Max(item => item.StockAmount.ToString().Length), cellpadding);

                //Draw Headers
                //Console.SetCursorPosition(1, Console.GetCursorPosition().Top);
                Helpers.WriteLineInColor(ConsoleColor.Blue,
                    headerId.PadRight(padId) +
                    headerName.PadRight(padName) +
                    headerDescription.PadRight(padDescription) +
                    headerCategoryId.PadRight(padCategoryId) +
                    headerSupplierName.PadRight(padSupplierName) +
                    headerUnitPrice.PadRight(padUnitPrice) +
                    headerUnitSalePrice.PadRight(padUnitSalePrice) +
                    headerOnSale.PadRight(padOnSale) +
                    headerStockAmount.PadRight(padStockAmount)
                    );

                //Draw Rows
                foreach (var product in products)
                {
                    //Console.SetCursorPosition(1, Console.GetCursorPosition().Top);
                    Console.WriteLine(
                        product.Id.ToString().PadRight(padId) + 
                        product.Name.PadRight(padName) +
                        product.Description.PadRight(padDescription) +
                        product.CategoryId.ToString().PadRight(padCategoryId) +
                        product.SupplierName.PadRight(padSupplierName) +
                        product.UnitPrice.ToString().PadRight(padUnitPrice) +
                        product.UnitSalePrice.ToString().PadRight(padUnitSalePrice) +
                        product.IsOnSale.ToString().PadRight(padOnSale) +
                        product.StockAmount.ToString().PadRight(padStockAmount)
                        );
                }
            }
            else
            {
                //Console.SetCursorPosition(1, Console.GetCursorPosition().Top);
                Console.WriteLine("List is empty...");
            }

        }


        public static void AddProduct()
        {
            using (var db = new Connections.WebShopContext())
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


                Product newProduct = new Product(name, description, categoryId, supplierName, unitPrice, stockAmount);
                if (ValidateProduct(newProduct))
                {
                    Console.Write("\n[Y] to add prodcut Or cancle [Any key]\n");
                    if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Y")
                    {
                        try
                        {
                            db.Products.Add(newProduct);
                            db.SaveChanges();

                            UserAction userAction = new UserAction(Settings.GetCurrentCustomerId(), Enums.UserActions.Product, "Added product Id: " + newProduct.Id);
                            MongoDbServices.AddUserActionAsync(userAction);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey(true);
                        }
                    }
                    else { Helpers.MsgLeavingAnyKey();}
                }
                else { Helpers.MsgBadInputsAnyKey();}
            }
        }


        /// <summary>
        /// stock += amount
        /// </summary>
        public static void UpdateProductStock(int productId, int amount)
        {
            Product product;
            using (var db = new Connections.WebShopContext())
            {
                product = db.Products.Where(p => p.Id == productId).SingleOrDefault();
                
                product.StockAmount += amount;
                
                db.SaveChanges();
            }
        }


        /// <summary>
        /// Sets stock. Stock = amount
        /// </summary>
        public static void SetProductStock(Product product, int amount)
        {
            using (var db = new Connections.WebShopContext())
            {
                product.StockAmount = amount;
                db.Products.Update(product);
                db.SaveChanges();
            }
        }


        public static Product SelectProduct()
        {
            
            PrintProducts(GetAllProducts());

            Console.Write("\nSelect Product... Enter ID: ");
            string input = Console.ReadLine();
            bool isNumber = int.TryParse(input, out int id) && id > 0;

            Product product = null;
            if (isNumber)
            {
                using (var db = new Connections.WebShopContext())
                {
                    product = db.Products.Where(p => p.Id == id).Include(p => p.Category).SingleOrDefault();
                }
            }
            return product;
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
                    Console.WriteLine(
                        $"Are you sure you want to DELETE: {product.Name}" +
                        $"\n[Y] or [Cancle any key]");

                    if(Console.ReadKey().Key.ToString().ToUpper() == "Y")
                        GenericServices.DeleteDbItem(product);
                }
            }
        }


        /// <summary>
        /// Gets product current price. Normal price or Sale price if on sale
        /// </summary>
        /// <returns></returns>
        public static decimal GetProductCurrentUnitPrice(Product product)
        {
            decimal price = 0;

            if (product.IsOnSale)
            {
                price = product.UnitSalePrice;
            }
            else
            {
                price = product.UnitPrice;
            }

            return price;
        }


        public static void UpdateProduct()
        {
            List<Product> products = GetAllProducts();
            PrintProducts(products);

            //Select product
            Console.Write("\nChoose Product - Enter ID: ");
            bool isValidId = int.TryParse(Console.ReadLine(), out int id) && id > 0;
            Product product = products.Where(item => item.Id == id).SingleOrDefault();

            Console.Clear();
            if (product != null)
            {
                bool isActive = true;
                while (isActive)
                {
                    //Print menu and selected Prodcut
                    Helpers.DrawMenuEnum(new Enums.UpdateProduct(), "Update Product");
                    PrintProducts(new List<Product> { GetProductById(product.Id) }); //Refresh product incase changes are made
                    Console.WriteLine("\n\n");

                    //Handle input
                    string input = Console.ReadKey(true).KeyChar.ToString();
                    if (int.TryParse(input, out int number) && number <= Enum.GetNames(typeof(Enums.UpdateProduct)).Length) //Check number is less than enum menu length
                    {
                        UpdateProductHandler(product, (Enums.UpdateProduct)number);
                    }
                    else
                    {
                        isActive = false;
                    }

                    Console.Clear();
                }
            }
        }

        public static async Task UpdateProductHandler(Product existingProduct, Enums.UpdateProduct enumOption)
        {
            if(enumOption == Enums.UpdateProduct.Update_Is_On_Sale) //If bool
            {
                Console.Write($"{enumOption.ToString().Replace("Update_", " ")} [Y] or [N]");
            }
            else //If string
            {
                Console.Write("Enter new" + enumOption.ToString().Replace("Update_", " ") + ": ");
            }
            
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Trim();

                using (var db = new Connections.WebShopContext())
                {
                    //var myTransaction = await db.Database.BeginTransactionAsync(); //Start transaction
                    try
                    {
                        UserAction userAction = new UserAction() { CustomerId = Settings.GetCurrentCustomerId(), Action = Enums.UserActions.Product.ToString()};
                        switch (enumOption)
                        {
                            case Enums.UpdateProduct.Update_Name:
                                userAction.Details = $"{enumOption}: {existingProduct.Name} : {input}"; //Generate MongoLog text
                                existingProduct.Name = input; //Update db item
                                
                                break;

                            case Enums.UpdateProduct.Update_Description:
                                userAction.Details = $"{enumOption}: {existingProduct.Description} : {input}";
                                existingProduct.Description = input;

                                break;

                            case Enums.UpdateProduct.Update_Category_Id:
                                userAction.Details = $"{enumOption}: {existingProduct.CategoryId} : {input}";
                                existingProduct.CategoryId = int.Parse(input);

                                break;

                            case Enums.UpdateProduct.Update_Supplyer_Name:
                                userAction.Details = $"{enumOption}: {existingProduct.SupplierName} : {input}";
                                existingProduct.SupplierName = input;

                                break;

                            case Enums.UpdateProduct.Update_Unit_Price:
                                userAction.Details = $"{enumOption}: {existingProduct.UnitPrice} : {input}";
                                existingProduct.UnitPrice = decimal.Parse(input.Replace('.', ','));

                                break;

                            case Enums.UpdateProduct.Update_Unit_Sale_Price:
                                userAction.Details = $"{enumOption}: {existingProduct.UnitSalePrice} : {input}";
                                existingProduct.UnitSalePrice = decimal.Parse(input.Replace('.', ','));

                                break;

                            case Enums.UpdateProduct.Update_Is_On_Sale:
                                if(input.ToUpper() == "Y")
                                {
                                    userAction.Details = $"{enumOption}: {existingProduct.IsOnSale} : {"Added to Sale"}";

                                    existingProduct.IsOnSale = true;

                                    //Safety: If sale price is higher than uit price or below 0:  Set sale price to Unitprice
                                    if (existingProduct.UnitSalePrice > existingProduct.UnitPrice || existingProduct.UnitSalePrice == 0) 
                                        existingProduct.UnitSalePrice = existingProduct.UnitPrice;
                                }
                                else if(input.ToUpper() == "N")
                                {
                                    userAction.Details = $"{enumOption}: {existingProduct.IsOnSale} : {"Removed from sale"}";
                                    existingProduct.IsOnSale = false;
                                }
                                break;

                            case Enums.UpdateProduct.Update_Stock:
                                userAction.Details = $"{enumOption}: {existingProduct.StockAmount} : {input}";
                                existingProduct.StockAmount = int.Parse(input);

                                break;
                        }


                        db.Update(existingProduct);
                        db.SaveChanges();
                        //await myTransaction.CommitAsync();

                        await MongoDbServices.AddUserActionAsync(userAction);
                    }
                    catch (Exception ex)
                    {
                        //await myTransaction.RollbackAsync();
                        Console.WriteLine("Could not update " + enumOption.ToString().Replace("Update_", " ") + "\n");
                        Console.WriteLine(ex.Message);
                        Console.ReadKey(true);

                        
                    }
                }
            }
        }

        public static bool ValidateProduct(Product product)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(product.Name)) return false;

            if (string.IsNullOrWhiteSpace(product.Description)) return false;

            if (product.CategoryId !<= 0) return false;

            if (string.IsNullOrWhiteSpace(product.SupplierName)) return false;

            if (product.UnitPrice !<= 0) return false;

            if (product.StockAmount < 0) return false; //Allow valid if zero


            return isValid;
        }
    }
}
