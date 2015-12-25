using System.Net.Sockets;
using System.Text;

namespace PS03.Network.Transmit
{
    public class RTransLink
    {
        public string ip;
        public int port;
        private TcpClient tcpclnt;

        public RTransLink(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            
        }

        public void Send(string tosend)
        {
            tcpclnt = new TcpClient();
            tcpclnt.SendTimeout = 600000;
            tcpclnt.Connect(ip, port);
            var data = Encoding.ASCII.GetBytes(tosend);
            var stream = tcpclnt.GetStream();
            stream.Write(data, 0, data.Length);
            tcpclnt.Close();
        }
    }
}