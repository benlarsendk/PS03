using System;
using System.Net.Sockets;
using System.Text;

namespace PS03.Transmit
{
    public class RTransLink
    {
        public string ip = "192.168.1.77";
        public int port = 501;
        private TcpClient tcpclnt;

        public void Send(string tosend)
        {
            tcpclnt = new TcpClient();
            tcpclnt.Connect(ip, port);
            var data = Encoding.ASCII.GetBytes(tosend);
            var stream = tcpclnt.GetStream();
            stream.Write(data, 0, data.Length);
            tcpclnt.Close();
        }
    }
}