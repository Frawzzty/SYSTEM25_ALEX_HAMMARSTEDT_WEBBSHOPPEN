using System.Globalization;
using WebShop.Menus;
using WebShop.Windows;

namespace WebShop
{
    internal class Program
    {
        //public static int myCustomerId;

        static async Task Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE"); //Resolves Dapper issues. (Wrong date foramting depeinging on keyboard lang settings etc)

            //Testing.WindowTesting();

            //BEFORE START
            //IF Crash at launch: IP adress likely not whitelisted. main DB and or MongoDB
                //CHECK DB STRINGS in user secrets;
                //CHECK VPN is OFF / check ipadress is whitelisted

            while (true)
            {
                bool auth = false;
                if (Settings.isDebugEnabled()) //IF debug enabled skip login screen
                {
                    auth = true;
                    Settings.SetCurrentCustomer(1); //Enter custmer ID with Admin privileges
                }
                else 
                {
                    auth = await WindowLoginRegister.AuthenticateAsync(); //Login or Register
                }

                if (auth)
                {
                    MenuHome.MenuHomeMain();
                }

            }
        }
    }
} 