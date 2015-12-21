namespace PS03
{
    public class GetWifi
    {
        public void Execute()
        {
            var ev = new Eventhandlers();
            WiPS.WiPS.OnWifiProflesReady += ev.HandleWiFiProfileReady;
            WiPS.WiPS.Execute();
        }
    }
}