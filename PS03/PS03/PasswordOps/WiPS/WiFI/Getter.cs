using System.Collections.Generic;
using PS03.PasswordOps.WiPS.CmdLineExecutor;
using PS03.PasswordOps.WiPS.Models;

namespace PS03.PasswordOps.WiPS.WiFI
{
    internal class Getter
    {
        private readonly Executor _executor = new Executor();
        private readonly CmdCommand _profilesCmd = new CmdCommand("netsh wlan show profiles");

        public void GetProfiles(List<Profile> ProfilesList)
        {
            var dop = new DOP(ProfilesList);
            _profilesCmd.OnOutputReady += dop.ProfilesReadyHandler;
            _executor.AddCommand(_profilesCmd);
            _executor.ExecuteCommands();
        }
    }
}