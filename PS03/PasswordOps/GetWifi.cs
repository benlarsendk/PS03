namespace PS03.PasswordOps
{
    public class GetWifi
    {
        private readonly string _ip;
        private readonly int _port;
        private readonly bool trans;
        private readonly bool verbose;

        public GetWifi(string ip, int port, bool t, bool v)
        {
            trans = t;
            verbose = v;
            _ip = ip;
            _port = port;
        }

        public void Execute()
        {
            var ev = new Eventhandlers(_ip, _port, trans, verbose);
            WiPS.WiPS.OnWifiProflesReady += ev.HandleWiFiProfileReady;
            WiPS.WiPS.Execute();
        }
    }
}