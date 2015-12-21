using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalSecurity.WiFi;
using WiPS.Modles;

namespace WiPS
{
   
    public class WiPS
    {
        public static List<Profile> ProfilesList { get; set; } = new List<Profile>();
        public delegate void OutputReadyHandler(List<Profile> data);
        public static event OutputReadyHandler OnWifiProflesReady;
        public static void HandleOutputReady(List<Profile> output)
        {
            OnWifiProflesReady?.Invoke(output);
        }

        public static void Execute()
        {
            var getter = new Getter();
            getter.GetProfiles(ProfilesList);
            HandleOutputReady(ProfilesList);
        }


    }
}
