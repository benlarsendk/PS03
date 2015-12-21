using System.Collections.Generic;
using WiPS.CmdLineExecutor;
using WiPS.Modles;
using WiPS.WiFi;

namespace GlobalSecurity.WiFi
{
    internal class Getter
    {
        private CmdCommand _profilesCmd = new CmdCommand("netsh wlan show profiles");
        private Executor _executor = new Executor();

        public void GetProfiles(List<Profile> ProfilesList)
        {
            DOP dop = new DOP(ProfilesList);
            _profilesCmd.OnOutputReady += dop.ProfilesReadyHandler;
            _executor.AddCommand(_profilesCmd);
            _executor.ExecuteCommands();
        }

    }
}
