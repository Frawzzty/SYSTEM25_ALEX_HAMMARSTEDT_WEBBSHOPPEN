using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Enums
{
    internal enum ShippingOptions //Name, Price SEK
    {
        Post_Nord = 65,
        Instabox = 55,
    }

    internal enum PaymentOptions
    {
        Card = 1,
        Klarna,
    }

    internal enum UserActions
    {
        Added_To_Cart,
        Remove_From_Cart,
        Pruchase,
        Pruchase_Failed,
        Customer_Added,
        Customer_Removed,
        Customer_Updated,
        Product,
        Logged_In,
    }

    internal enum UpdateCustomer
    {
        Update_Name = 1,
        Update_street,
        Update_city,
        Update_Country,
        Update_Email,
        Update_Password
    }

    internal enum UpdateProduct
    {
        Update_Name = 1,
        Update_Description,
        Update_Category_Id,
        Update_Supplyer_Name,
        Update_Unit_Price,
        Update_Unit_Sale_Price,
        Update_Is_On_Sale,
        Update_Stock
    }
}
