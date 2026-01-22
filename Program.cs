using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebShop.DbServices;
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

        static async Task Main(string[] args)
        {

            //Helpers.GetDates(7);
            

            //BEFORE START
            //Crashes at launch likley unallowed IP adress.
            //CHECK VPN is OFF;
            //CHECK DB STRINGS;


            //Testing.WindowTesting();

            //Console.CursorVisible = false;
            //Settings.SetCurrentCustomer(WindowCustomer.SelectCustomer());
            Console.Clear();


            while (true)
            {
                bool auth = false;
                if (Settings.GetDebugStatus())
                {
                    auth = true;
                    Settings.SetCurrentCustomer(2);
                }
                else 
                {
                    auth = await WindowLoginRegister.Authenticate(); //Login or Register
                }

                    
                if (auth) 
                {
                    MenuHome.MenuHomeMain();
                }

            }

            Console.WriteLine("Byyyye, thanks for moooney! <3 \n\n\n");

        }
    }
} 