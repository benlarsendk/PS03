using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.Presentation
{
    public class Presenter
    {
        public void ShowWelcome()
        {
            var data = " ============== PS03 - PasswordSniffer V3.3 ============== ";
            var under = "=========================";
            var quote =
                "\"Passwords are like underwear; you don’t let people see it, you should change it very often, and you shouldn’t share it with strangers\" – Chris Pirill";

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
            Console.SetCursorPosition((Console.WindowWidth - under.Length) / 2, Console.CursorTop);
            Console.WriteLine(under);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - 80) / 2, Console.CursorTop);
            Console.WriteLine(quote);
            Console.WriteLine();
            Console.WriteLine();

            Console.ResetColor();

        }

        public void ShowTargets()
        {

        }
    }
}
