using System.Collections.Generic;
using PS03.PasswordOps.WiPS.Models;
using PS03.PasswordOps.WiPS.WiFI;

namespace PS03.PasswordOps.WiPS
{
    public class WiPS
    {
        public delegate void OutputReadyHandler(List<Profile> data);

        public static List<Profile> ProfilesList { get; set; } = new List<Profile>();
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