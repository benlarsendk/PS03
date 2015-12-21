using System.Collections.Generic;
using WiPS.CmdLineExecutor;
using WiPS.Modles;

namespace WiPS.WiFi
{
    internal class DOP
    {
        private readonly List<Profile> ProfilesList;

        public DOP(List<Profile> profilesLIst)
        {
            ProfilesList = profilesLIst;
        }

        private void GetPasswords()
        {
            var executor = new Executor();
            foreach (var profile in ProfilesList)
            {
                var _dataCmd = new CmdCommand("netsh wlan show profil name=\"" + profile.SSID + "\"" + " key=clear");
                _dataCmd.OnOutputReady += ProfileDataReadyHandler;
                executor.AddCommand(_dataCmd);
            }

            executor.ExecuteCommands();
        }

        public void ProfilesReadyHandler(string data)
        {
            var rawprofiles = GetRawProfileLines(GetLines(data));
            foreach (var rawpro in rawprofiles)
            {
                ProfilesList.Add(GetProfileNameSSID(rawpro));
            }

            foreach (var VARIABLE in ProfilesList)
            {
                GetPasswords();
            }
        }

        private List<string> GetLines(string data)
        {
            var lines = new List<string>();
            var line = "";
            foreach (var x in data)
            {
                if (x == '\n')
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                    line = "";
                }
                else
                {
                    line += x;
                }
            }

            return lines;
        }

        private List<string> GetRawProfileLines(List<string> lines)
        {
            var rawprofiles = new List<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("    All User Profile"))
                {
                    rawprofiles.Add(line);
                }
            }
            return rawprofiles;
        }


        private Profile GetProfileNameSSID(string rawprofile)
        {
            var sep = rawprofile.IndexOf(": ") + ": ".Length;
            return new Profile { SSID = rawprofile.Substring(sep, rawprofile.Length - sep - 1) };
        }

        private Profile SortLines(List<string> lines)
        {
            var p = new Profile();
            foreach (var line in lines)
            {
                if (line.StartsWith("    Authentication"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.Encryption = line.Substring(sep, line.Length - sep - 1);
                }
                else if (line.StartsWith("    Key Content"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.Password = line.Substring(sep, line.Length - sep - 1);
                }
                else if (line.StartsWith("    Name"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.SSID = line.Substring(sep, line.Length - sep - 1);
                }
            }
            return p;
        }

        private void Checkup(Profile p)
        {
            foreach (var pro in ProfilesList)
            {
                if (pro.SSID == p.SSID)
                {
                    pro.Password = p.Password;
                    pro.Encryption = p.Encryption;
                    break;
                }
            }
        }

        public void ProfileDataReadyHandler(string data)
        {
            var rawdata = SortLines(GetLines(data));
            Checkup(rawdata);
        }
    }
}