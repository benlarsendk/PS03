using System;
using System.Threading;

namespace PS03_Server
{
    public class WifiHandler
    {
        bool first = true;
        Mutex mtx = new Mutex();
        public void Handle(string data)
        {
            int ssiMark = 0;
            int encMark = 0;
            int pasMark = 0;

            ssiMark = data.IndexOf("SSID=") + ("SSID=").Length;
            encMark = data.IndexOf("ENC=");
            pasMark = data.IndexOf("PASS=");

            string SSID = data.Substring(ssiMark,encMark-ssiMark);
            string Encryption = data.Substring(encMark + ("ENC=").Length, pasMark - (encMark + ("ENC=").Length));
            string Password = data.Substring(pasMark + ("PASS=").Length, data.Length - (pasMark + ("PASS=").Length));

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
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;

        }
    }
}