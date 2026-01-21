using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebShop.Enums;
using WebShop.Menus;
using WebShop.Modles;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop
{
    internal class Program
    {
        //public static int myCustomerId;

        static void Main(string[] args)
        {

            //Console.ReadKey();


            //BEFORE START
            //CHECK VPN is OFF;
            //CHECK DB STRINGS;
            //Crashes at launch likley unallowed IP adress.

            //Testing.WindowTesting();

            //Console.CursorVisible = false;
            Settings.SetCurrentCustomer(WindowCustomer.SelectCustomer());
            Console.Clear();


            while (true)
            {
                //ChooseCustomer
                MenuHome.MenuHomeMain();
                break;
            }

            Console.WriteLine("Byyyye please dont ever come back!\n\n\n");

        }
    }
} 