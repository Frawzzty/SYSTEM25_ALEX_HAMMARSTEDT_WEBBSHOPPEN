using WebShop.Services;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuHome
    {
        public static void MenuHomeMain()
        {
            string menuHeader = "Home";
            bool loop = true;

            while (loop)
            {
                //Draw menu and Home screen
                Helpers.DrawMenuEnum(new Enums.MenuHomeMain(), menuHeader);
                WindowHome.DrawHome();

                //Draw product windows
                List<Models.Product> productsOnSale = ProductServices.GetProductsOnSale().Where(p => p.StockAmount > 0).ToList();
                List<string> saleActionKeys = Helpers.GetActionKeys().Take(4).ToList(); //Get actions keys for prodcut windows. Take(x) to limit how many windows are drawn.
                WindowSaleProduct.DrawProductWindows(productsOnSale, saleActionKeys.Count, saleActionKeys);

                //Menu Inputs
                string input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int actionKeyIndex = Helpers.GetActionKeyIndex(input); //Check if input was a valid key for prodcut windows
                
                Console.Clear();
                //Navbar menu
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuHomeMain)number)
                    {
                        case Enums.MenuHomeMain.Store:
                            Menus.MenuStore.MenuStoreMainDynamic();
                            break;

                        case Enums.MenuHomeMain.Cart:
                            Menus.MenuCart.MenuCartMain();
                            break;

                        case Enums.MenuHomeMain.Order_History:
                            Menus.MenuOrderHistory.MenuOrderHistoryMain();
                            break;

                        case Enums.MenuHomeMain.Logout:
                            Settings.SetCurrentCustomer(-1); //Set current customer to invalid id
                            loop = false;
                            break;

                        case Enums.MenuHomeMain.Admin:
                            if (Settings.GetCurrentCustomer().IsAdmin) //Admin lock
                                Menus.MenuAdmin.MenuAdminMain();
                            
                            break;

                        case Enums.MenuHomeMain.Exit:
                            Environment.Exit(0);
                            break;
                    }
                }
                //Add to cart
                else if(actionKeyIndex >= 0) //Try add sale product to cart
                {
                    Helpers.AddProductOnSaleToCart(productsOnSale, actionKeyIndex);
                }

                Console.Clear();
            }
        }


    }
}
