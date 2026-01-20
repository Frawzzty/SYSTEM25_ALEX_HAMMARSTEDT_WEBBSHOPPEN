

using Microsoft.EntityFrameworkCore.Update.Internal;

namespace WebShop
{
    public class Window
    {
        public string Header { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public List<string> TextRows { get; set; }

        public ConsoleColor headerColor { get; set; }

        public Window(string header, int left, int top, List<string> textRows)
        {
            Header = header;
            Left = left;
            Top = top;
            TextRows = textRows;
            headerColor = ConsoleColor.White;
        }

        public void Draw()
        {
            var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;

            // Kolla om Header är längre än det längsta ordet i listan
            if (width < Header.Length + 4)
            {
                width = Header.Length + 4;
            }
        ;

            // Rita Header
            Console.SetCursorPosition(Left, Top);
            if (Header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = headerColor;
                Console.Write(Header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new string('─', width - Header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new string('─', width + 2) + '┐');
            }

            // Rita raderna i sträng-Listan
            for (int j = 0; j < TextRows.Count; j++)
            {
                Console.SetCursorPosition(Left, Top + j + 1);
                Console.WriteLine('│' + " " + TextRows[j] + new string(' ', width - TextRows[j].Length + 1) + '│');
            }

            // Rita undre delen av fönstret
            Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
            Console.Write('└' + new string('─', width + 2) + '┘');


            // Kolla vilket som är den nedersta posotion, i alla fönster, som ritats ut
            if (Lowest.LowestPosition < Top + TextRows.Count + 2)
            {
                Lowest.LowestPosition = Top + TextRows.Count + 2;
            }

            Console.SetCursorPosition(0, Lowest.LowestPosition);
        }

        public void Draw(ConsoleColor headerColor)
        {
            var width = TextRows.OrderByDescending(s => s.Length).FirstOrDefault().Length;

            // Kolla om Header är längre än det längsta ordet i listan
            if (width < Header.Length + 4)
            {
                width = Header.Length + 4;
            }
        ;

            // Rita Header
            Console.SetCursorPosition(Left, Top);
            if (Header != "")
            {
                Console.Write('┌' + " ");
                Console.ForegroundColor = headerColor;
                Console.Write(Header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + new string('─', width - Header.Length) + '┐');
            }
            else
            {
                Console.Write('┌' + new string('─', width + 2) + '┐');
            }

            // Rita raderna i sträng-Listan
            for (int j = 0; j < TextRows.Count; j++)
            {
                Console.SetCursorPosition(Left, Top + j + 1);
                Console.WriteLine('│' + " " + TextRows[j] + new string(' ', width - TextRows[j].Length + 1) + '│');
            }

            // Rita undre delen av fönstret
            Console.SetCursorPosition(Left, Top + TextRows.Count + 1);
            Console.Write('└' + new string('─', width + 2) + '┘');


            // Kolla vilket som är den nedersta posotion, i alla fönster, som ritats ut
            if (Lowest.LowestPosition < Top + TextRows.Count + 2)
            {
                Lowest.LowestPosition = Top + TextRows.Count + 2;
            }

            Console.SetCursorPosition(0, Lowest.LowestPosition);
        }


        public static int GetWindowHorizontalLength(Window window) 
        {
            int length = 1;
            int accountForBorder = 4;
            foreach(string row in window.TextRows)
            {
                if (length < row.Length)
                {
                    length = row.Length;
                }
            }

            if(window.Header.Length > length)
            {
                length = window.Header.Length + 4;
            }
   
            return (length + accountForBorder);
        }

        public static int GetWindowVerticalLength(Window window)
        {
            int length = 1;
            int accountForBorder = 2;
            length = window.TextRows.Count;

            return (length + accountForBorder);
        }

        public static void DrawWindowsInRow(List<Window> windows, int startLeft, int startTop, int extraWindowGap)
        {

            foreach (Window window in windows) 
            {
                window.Left = startLeft;
                window.Top = startTop;
                window.Draw();
                startLeft += GetWindowHorizontalLength(window) + extraWindowGap;
            }
        }

        public static void DrawWindowsInColumn(List<Window> windows, int startLeft, int startTop, int extraWindowGap)
        {

            foreach (Window window in windows)
            {
                window.Left = startLeft;
                window.Top = startTop;
                window.Draw();
                startTop += GetWindowVerticalLength(window) + extraWindowGap;
            }
        }
    }

    

    public static class Lowest
    {
        public static int LowestPosition { get; set; }
    }
}
