using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Menus;
using WebShop.Models;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop
{
    internal class Program
    {
        //public static int myCustomerId;

        static async Task Main(string[] args)
        {

            //Helpers.GetDates(7);
            //Testing.WindowTesting();

            //BEFORE START
            //Crashes at launch likley IP adress no whitelisted.
            //CHECK VPN is OFF;
            //CHECK DB STRINGS;

            Console.Clear();

            while (true)
            {
                bool auth = false;
                if (Settings.GetDebugStatus()) //IF debug enabled skip login screen
                {
                    auth = true;
                    Settings.SetCurrentCustomer(1);
                }
                else 
                {
                    auth = await WindowLoginRegister.AuthenticateAsync(); //Login or Register
                }

                    
                if (auth)  //If auth scuess
                {
                    MenuHome.MenuHomeMain();
                }

            }

        }
    }
} 