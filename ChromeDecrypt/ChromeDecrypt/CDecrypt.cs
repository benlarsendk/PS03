using System.Text;

namespace ChromeDecrypt
{
    public class CDecrypt
    {
        public string decrypt(byte[] encBytes)
        {
            byte[] entropy = null;
            string description;

            var decrypted = DPAPI.Decrypt(encBytes, entropy, out description);
            var password = new UTF8Encoding(true).GetString(decrypted);

            return password;
        }
    }
}