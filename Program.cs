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
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("sv-SE"); //Resolves Dapper issues. (Wrong date foramting depending on keyboard lang settings etc)

            //Testing.WindowTesting();

            //BEFORE START
            //IF Crash at launch: IP adress likely not whitelisted. main DB and or MongoDB
                //CHECK DB STRINGS in user secrets;
                //CHECK VPN is OFF / check ipadress is whitelisted

            //IF first time user, can disable admin menu lock in MenuHome.MenuHomeMain() //Approx Row 64

            while (true)
            {
                bool auth = false;
                //IF debug enabled skip login screen
                if (Settings.isDebugEnabled()) 
                {
                    auth = true;
                    Settings.SetCurrentCustomer(1); //Enter custmer ID with Admin privileges
                }
                //Login or Register
                else
                {
                    auth = await WindowLoginRegister.AuthenticateAsync(); 
                }

                if (auth)
                {
                    //Context: Menus are used for navigation. Menues Load Windows.cs instances
                    MenuHome.MenuHomeMain();
                }

            }
        }
    }
} 