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
        Pruchase_Success,
        Pruchase_Failed,
        Customer_Added,
        Customer_Removed,
        Logged_In,
        Logged_Out
    }
}
