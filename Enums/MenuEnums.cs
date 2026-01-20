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
        Order_History,
        Switch_Customer,
        Admin,
        Exit = 9
    }

    //--MENU ORDER HISTORY--------------------------------------------------------------------------//




    //--ADMIN MENU-----------------------------------------------------------------------------//
    public enum MenuAdminMain
    {
        Product = 1,
        Category, 
        Customers,
        Dashboard,
        Back = 9,
    }

    public enum MenuAdminProduct
    {
        Add_Product = 1,
        Update_Product,
        Edit_Sale,
        Update_Stock,
        Delete_Product,

        Back = 9,
    }
    public enum MenuAdminUpdateProduct
    {
        Edit_Name,
        Edit_Desciption,
        Edit_Category,
        Edit_Supplier,
        Edit_Unit_Price,
        Set_Stock,

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


    //--CART MENU---------------------------------------------------------------------------//
    public enum MenuCartMain
    {
        Next_Shipping = 1,
        Edit_Cart,

        Back = 9,
    }
    public enum MenuCartEdit
    {
        Previous_Item = 1,
        Next_Item = 2,
        Decrease = 4,
        Increase = 5,
        

        Back = 9,
    }

    //--CART MENU---------------------------------------------------------------------------//

    public enum MenuCheckOutMain
    {
        Shipping_Info = 1,
        Payment_Info,
        Pay,

        Back = 9,
    }

    public enum MenuCheckoutShipping
    {
        Enter_Manually = 1,
        Auto_Fill,

    }

    public enum MenuCheckoutPayment
    {
        Enter_Manually = 1,
        Auto_Fill,

        Back = 9,
    }
}
