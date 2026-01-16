using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Enums
{
    internal enum ShippingOption //Name, Price SEK
    {
        Post_Nord = 65,
        Instabox = 55,
    }

    internal enum PaymentOption
    {
        Card = 1,
        Klarna,
    }
}
