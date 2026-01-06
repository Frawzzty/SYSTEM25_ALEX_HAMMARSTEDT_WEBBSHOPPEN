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
        static void Main(string[] args)
        {
            //Console.CursorVisible = false;


            while (true)
            {
                MenuHome.MenuHomeMain();
                break;
            }

            Console.WriteLine("Byyyye please dont ever come back!");



            //FIXA DB LISTOR FÖR ALLA EN TILL MÅNGA
        }

    }
} 