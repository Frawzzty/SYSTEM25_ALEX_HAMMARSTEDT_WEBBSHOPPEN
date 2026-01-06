using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Enums
{

    public enum MenuHome
    {
        Store = 1,
        Cart,                       // --> show product --> Shipping --> Payment
        Admin,
        Exit = 9
    }
    //----------------------------------------------------------------------------------//
    

    public enum MenuAdminMain
    {
        Product = 1,                //Product ändringar
        Category,                   //Kategori ändringar
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

    public enum MenuAdminClient
    {
        Order_History = 1,          //Product ändringar
        Update_Name,                //??
        Back = 9,
    }

}
