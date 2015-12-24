using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS03
{
    class Program
    {
        static void Main(string[] args)
        {
            var Wifi = new GetWifi();
            var Chrome = new GetChrome();
            var Firefox = new GetFirefox();


            Thread T_WIFI = new Thread(Wifi.Execute);
            Thread T_CHROME = new Thread(Chrome.Execute);
            Thread T_FIREFOX = new Thread(Firefox.Execute);


            T_WIFI.Start();
            T_CHROME.Start();  
            T_FIREFOX.Start();

            T_WIFI.Join();
            T_CHROME.Join();
            T_FIREFOX.Join();

        }


    }
}
