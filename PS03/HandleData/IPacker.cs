using PS03.HandleData;
using System;

namespace PS03
{
    public interface IPacker
    {
        string Pack(Profile Profile);
        Profile Unpack(string s);
    }

    public class PasswordPacker : IPacker
    {

        public string Pack(Profile profile)
        {
            SimpleAES enc = new SimpleAES();

            var data = profile.AppName + ":";
            if (!string.IsNullOrWhiteSpace(profile.Action))
                data += ("ACTION=" + profile.Action);
            else data += ("ACTION=EMPTY");

            if (!string.IsNullOrWhiteSpace(profile.Specification))
                data += ("SPECIFICATION=" + profile.Specification);
            else data += "SPECIFICATION=EMPTY";

            if (!string.IsNullOrWhiteSpace(profile.Password))
                data += ("PASS=" + profile.Password);
            else data += "PASS=EMPTY";

            return enc.EncryptToString(data);
        }

        public Profile Unpack(string dataEnc)
        {
             SimpleAES enc = new SimpleAES();

            string data = enc.DecryptString(dataEnc);

            Profile p = new Profile();
            var actMark = 0;
            var specMark = 0;
            var pasMark = 0;
            var AppMark = 0;

            actMark = data.IndexOf("ACTION=") + ("ACTION=").Length;
            specMark = data.IndexOf("SPECIFICATION=");
            pasMark = data.IndexOf("PASS=");
            AppMark = data.IndexOf(":");

            p.AppName = data.Substring(0, AppMark);
            p.Action = data.Substring(actMark, specMark - actMark);
            p.Specification= data.Substring(specMark + ("SPECIFICATION=").Length, pasMark - (specMark + ("SPECIFICATION=").Length));
            p.Password = data.Substring(pasMark + ("PASS=").Length, data.Length - (pasMark + ("PASS=").Length));

            return p;

        }
    }
}