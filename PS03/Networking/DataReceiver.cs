using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03
{
    public class DataReceiver
    {
        CommandLineOptions.Options _options;
        public DataReceiver(CommandLineOptions.Options options)
        {
            _options = options;
        }
        public void BeginReceive()
        {
            var datahandler = new HandleData.DataHandler(_options);
            INetworkHandler networkhandler = new TCPNetworkHandler(_options.Port);

            networkhandler.DataReady += datahandler.HandleReceivedData;
            networkhandler.VictimConnected += datahandler.HandleVictimConected;

            networkhandler.Receive();

        }
    }
}
