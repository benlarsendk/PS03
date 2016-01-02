using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03
{
    public interface IPrinter
    {
        void Print(string data);
    }

    public class FilePrinter : IPrinter
    {
        public void Print(string data)
        {
            File.WriteAllText("Report_PS03" + ".html", data);
        }
    }

    public class ConsolePrinter : IPrinter
    {
        public void Print(string data)
        {
           // Console.WriteLine(data);
        }


     
    }
}
