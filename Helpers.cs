using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop
{
    internal class Helpers
    {
        public static void WriteInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }

        //Messages
        public static void MessageLeavingAnyKey()
        {
            Console.WriteLine("Leaving, nothing updated... Any key to continue");
            Console.ReadKey(true);
        }

        public static void MessageContinueAnyKey()
        {
            Console.WriteLine("Any key to continue...");
            Console.ReadKey(true);
        }

        public static void MessageBadInputsAnyKey()
        {
            Console.WriteLine("Bad inputs. Any key to continue...");
            Console.ReadKey(true);
        }

        public static int GetHeaderMaxPadding(string header, int textLength, int columnSpacing)
        {
            int padding = 0;
            if(textLength >= header.Length)
            {
                padding = textLength;
            }
            else
            {
                padding = header.Length;
            }
            return padding + columnSpacing;
        }

        public static int GetProdcutWindowLeftLength(List<string> windowTexts)
        {
            int windowSize = 0;
            foreach (string windowText in windowTexts) 
            {
                if (windowText.Length >= windowSize)
                {
                    windowSize = windowText.Length;
                }
            }
            
            return windowSize;
        }
    }
}
