using System;
using System.Threading;
using PS03.Network.Receive.PostOp;

namespace PS03.Network.Receive.Handlers
{
    public class WifiHandler
    {
        private bool first = true;
        private readonly Mutex mtx = new Mutex();
        private readonly PasswordCounter pwc;

        public WifiHandler(PasswordCounter PWC)
        {
            pwc = PWC;
        }

        public void Handle(string data)
        {
            var ssiMark = 0;
            var encMark = 0;
            var pasMark = 0;

            ssiMark = data.IndexOf("SSID=", StringComparison.Ordinal) + ("SSID=").Length;
            encMark = data.IndexOf("ENC=", StringComparison.Ordinal);
            pasMark = data.IndexOf("PASS=", StringComparison.Ordinal);

            var SSID = data.Substring(ssiMark, encMark - ssiMark);
            var Encryption = data.Substring(encMark + ("ENC=").Length, pasMark - (encMark + ("ENC=").Length));
            var Password = data.Substring(pasMark + ("PASS=").Length, data.Length - (pasMark + ("PASS=").Length));
            pwc.Add(Password);
            Print(SSID, Encryption, Password);
        }

        private void Print(string ssid, string enc, string pass)
        {
            mtx.WaitOne();
            if (first)
            {
                Console.WriteLine();
                printCenterWhite("# ---------------------- WiFi Profiles ---------------------- #");
            }
            first = false;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("SSID:\t\t");
            printRed(ssid);
            Console.Write("Encryption:\t");
            printRed(enc);
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