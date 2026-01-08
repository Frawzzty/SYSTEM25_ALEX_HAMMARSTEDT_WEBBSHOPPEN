using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Enums
{
    //--MENU HOME--------------------------------------------------------------------------//
    public enum MenuHomeMain
    {
        Store = 1,
        Cart,    // --> show product --> Shipping --> Payment
        Admin,

        Exit = 9
    }
    

    //--ADMIN MENU-----------------------------------------------------------------------------//
    public enum MenuAdminMain
    {
        Product = 1,
        Category, 
        Customers,

        Back = 9,
    }

    public enum MenuAdminProduct
    {
        Add_Product = 1,
        Update_Product,
        Set_on_sale,
        Delete_Product,

        Back = 9,
    }

    public enum MenuAdminCategory
    {
        Create_Category = 1,        //Product ändringar
        Update_Category,
        Delete_Category,

        Back = 9,
    }

    public enum MenuAdminCustomer
    {
        Add_Customer = 1,
        Update_Customer,
        Order_History,
        Delete_Customer,
        Set_Role,
        
        Back = 9,
    }


    //--STORE MENU---------------------------------------------------------------------------//
    public enum MenuStoreMain
    {
        Pants = 1,
        Shirts,
        Shoes,
        Hats,
        Search_Product, //Dapper?

        Back = 9,
    }


}
