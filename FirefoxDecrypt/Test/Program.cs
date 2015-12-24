using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirefoxDecrypt;

namespace Test
{
    abstract class Program
    {
        static void Main(string[] args)
        {
            var d = new FDC();
            FDC.OnPasswordReady += PasswordsReadyHandler;
            d.Execute();
        }

        public static void PasswordsReadyHandler(List<FFData> output)
        {
            foreach (var VARIABLE in output)
            {
                Console.WriteLine(VARIABLE.Username);
            }
        }
    }



}
