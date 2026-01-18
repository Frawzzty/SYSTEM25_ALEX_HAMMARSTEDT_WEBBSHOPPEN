using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
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
            //Console.CursorVisible = false;

            //myCustomerId = WindowCustomer.SelectCustomer(); //Get selected customer or create a new one. Used to keep track of what customer is "signed in"
            Settings.SetCurrentCustomer(WindowCustomer.SelectCustomer());
            Console.Clear();



            while (true)
            {
                //ChooseCustomer
                MenuHome.MenuHomeMain();
                break;
            }

            Console.WriteLine("Byyyye please dont ever come back!");

        }
    }
} 